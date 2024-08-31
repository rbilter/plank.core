using System.Data;
using System.Globalization;
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Core.Contracts;
using Plank.Core.Data;
using Plank.Core.Models;
using Plank.Core.Profiles;
using Serialize.Linq.Serializers;

namespace Plank.Core.Services
{
    public sealed class PlankService<TEntity> : IService<TEntity> where TEntity : class, IEntity, new()
    {
        private readonly IRepository<TEntity> _repository;
        private readonly ILogger _logger;

        private readonly string _defaultErrorMessage;
        private readonly string _defaultItemNotFoundMessage;
        private readonly string _defaultNullParameterMessage;

        public PlankService(IRepository<TEntity> repository, ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _defaultErrorMessage = "There was an issue processing the request, see the plank logs for details";
            _defaultItemNotFoundMessage = "Item could not be found";
            _defaultNullParameterMessage = "Value cannot be null or empty.\r\nParameter name: {0}";
        }

        public async Task<PlankPostResponse<TEntity>> Add(TEntity item)
        {
            _logger.InfoMessage(item.ToJson());

            var validation = item.Validate();
            if (validation.IsValid)
            {
                try
                {
                    await _repository.Add(item).ConfigureAwait(false);
                }
                catch (DataException e)
                {
                    _logger.ErrorMessage(e);

                    validation.Add(new PlankValidationResult
                    {
                        Key = "Error",
                        Message = _defaultErrorMessage,
                        Target = this
                    });
                }
            }

            var results = new PlankPostResponse<TEntity>(validation)
            {
                Item = item
            };

            _logger.InfoMessage(results.ToJson());

            return results;
        }

        public async Task<PlankBulkPostResponse<TEntity>> BulkAdd(IEnumerable<TEntity> items)
        {
            _logger.InfoMessage(items.ToJson());

            var validation = items.Validate();
            if (validation.Any(l => l.ValidationResults.IsValid))
            {
                try
                {
                    var itemsToSave = validation.Where(l => l.ValidationResults.IsValid).Select(l => l.Item);
                    await _repository.BulkAdd(itemsToSave).ConfigureAwait(false);
                }
                catch (DataException e)
                {
                    _logger.ErrorMessage(e);

                    validation.ForEach(v => v.ValidationResults.Add(new PlankValidationResult
                    {
                        Key = "Error",
                        Message = _defaultErrorMessage,
                        Target = this
                    }));
                }
            }

            _logger.InfoMessage(validation.ToJson());

            return new PlankBulkPostResponse<TEntity>(validation);
        }

        public async Task<PlankDeleteResponse> Delete(int id)
        {
            _logger.InfoMessage(id);

            var validation = new ValidationResults();
            try
            {
                if (await _repository.Get(id).ConfigureAwait(false) != null)
                {
                    await _repository.Delete(id).ConfigureAwait(false);
                }
            }
            catch (DataException e)
            {
                _logger.ErrorMessage(e);

                validation.AddResult(new ValidationResult(_defaultErrorMessage, this, "Error", null, null));
            }

            var results = new PlankDeleteResponse(Mapping<TEntity>.Mapper.Map<PlankValidationResultCollection>(validation))
            {
                Id = id
            };

            _logger.InfoMessage(results.ToJson());
            
            return results;
        }

        public async Task<PlankGetResponse<TEntity>> Get(int id)
        {
            _logger.InfoMessage(id);
            PlankGetResponse<TEntity> result;

            try
            {
                var item = await _repository.Get(id).ConfigureAwait(false);
                result = new PlankGetResponse<TEntity>(item)
                {
                    IsValid = true
                };
            }
            catch (DataException e)
            {
                _logger.ErrorMessage(e);
                result = new PlankGetResponse<TEntity>()
                {
                    IsValid = false,
                    Message = _defaultErrorMessage
                };
            }

            _logger.InfoMessage(result.ToJson());
            return result;
        }

        public async Task<PlankEnumerableResponse<TEntity>> Search(Expression<Func<TEntity, bool>> expression, List<Expression<Func<TEntity, object>>> includes, int pageNumber, int pageSize)
        {
            expression ??= (f => true);

            var serializer = new ExpressionSerializer(new JsonSerializer());
            _logger.InfoMessage(serializer.SerializeText(expression));

            PlankEnumerableResponse<TEntity> result = null;
            try
            {
                var pagedList = await _repository.Search(expression, includes, pageNumber, pageSize).ConfigureAwait(false);
                result = Mapping<TEntity>.Mapper.Map<PlankEnumerableResponse<TEntity>>(pagedList);
                result.IsValid = true;
            }
            catch (DataException e)
            {
                _logger.ErrorMessage(e);
                result = new PlankEnumerableResponse<TEntity>()
                {
                    IsValid = false,
                    Message = _defaultErrorMessage
                };
            }

            _logger.InfoMessage(result.ToJson());
            return result;
        }

        public async Task<PlankPostResponse<TEntity>> Update(TEntity item)
        {
            _logger.InfoMessage(item.ToJson());

            TEntity existing = null;
            var validation = item.Validate();
            if (validation.IsValid)
            {
                try
                {
                    existing = await _repository.Get(item.Id).ConfigureAwait(false);
                    if (existing != null)
                    {
                        foreach (var p in item.GetProperties())
                        {
                            p.SetValue(existing, p.GetValue(item));
                        };

                        await _repository.Update(existing).ConfigureAwait(false);
                    }
                    else
                    {
                        validation.Add(new PlankValidationResult
                        {
                            Key = "Error",
                            Message = _defaultItemNotFoundMessage,
                            Target = this
                        });
                    }
                }
                catch (DataException e)
                {
                    _logger.ErrorMessage(e);

                    validation.Add(new PlankValidationResult
                    {
                        Key = "Error",
                        Message = _defaultErrorMessage,
                        Target = this
                    });
                }
            }

            var results = new PlankPostResponse<TEntity>(validation)
            {
                Item = existing
            };

            _logger.InfoMessage(results.ToJson());

            return results;
        }

        public async Task<PlankPostResponse<TEntity>> Update(TEntity item, params Expression<Func<TEntity, object>>[] properties)
        {
            _logger.InfoMessage(item.ToJson());

            TEntity existing = null;

            var validation = new PlankValidationResultCollection();
            if (item == null)
            {
                var msg = string.Format(CultureInfo.InvariantCulture, _defaultNullParameterMessage, nameof(item));
                validation.Add(new PlankValidationResult
                {
                    Key = "Error",
                    Message = msg,
                    Target = this
                });
            }

            if (properties == null || properties.Any(p => (p.Body as MemberExpression ?? (p.Body as UnaryExpression)?.Operand as MemberExpression) == null))
            {
                var msg = string.Format(CultureInfo.InvariantCulture, _defaultNullParameterMessage, nameof(properties));
                validation.Add(new PlankValidationResult
                {
                    Key = "Error",
                    Message = msg,
                    Target = this
                });
            }

            if (validation.IsValid)
            {
                try
                {
                    existing = await _repository.Get(item.Id).ConfigureAwait(false);
                    if (existing != null)
                    {
                        // Assign values from item to the existing entity
                        foreach (var p in properties)
                        {
                            var operand = p.Body as MemberExpression ?? (p.Body as UnaryExpression).Operand as MemberExpression;
                            existing.GetType().GetProperty(operand.Member.Name).SetValue(existing, item.GetType().GetProperty(operand.Member.Name).GetValue(item));
                        }

                        // Assign values from existing back to item for validation
                        foreach (var p in item.GetProperties())
                        {
                            item.GetType().GetProperty(p.Name).SetValue(item, existing.GetType().GetProperty(p.Name).GetValue(existing));
                        }

                        validation = item.Validate();
                        if (validation.IsValid)
                        {
                            await _repository.Update(existing).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        validation.Add(new PlankValidationResult
                        {
                            Key = "Error",
                            Message = _defaultItemNotFoundMessage,
                            Target = this
                        });
                    }
                }
                catch (DataException e)
                {
                    _logger.ErrorMessage(e);

                    validation.Add(new PlankValidationResult
                    {
                        Key = "Error",
                        Message = _defaultErrorMessage,
                        Target = this
                    });
                }
            }

            var results = new PlankPostResponse<TEntity>(validation)
            {
                Item = existing
            };

            _logger.InfoMessage(results.ToJson());

            return results;
        }
    }
}