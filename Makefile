.PHONY: all
all: clean build test

.PHONY: build
build:
	dotnet build

.PHONY: clean
clean:
	dotnet clean

.PHONY: pack
pack:
	dotnet pack ./src/Plank.Core/Plank.Core.csproj -c Release -o ./artifacts

.PHONY: publish
publish: pack
	dotnet nuget push ./artifacts/Plank.Core.*.nupkg -k $(NUGET_API_KEY) -s https://api.nuget.org/v3/index.json --skip-duplicate

.PHONY: run-todo
run-todo:
	dotnet run --project ./tests/Todo.Api/Todo.Api.csproj

.PHONY: test
test:
	dotnet test ./tests/Plank.Core.Tests/Plank.Core.Tests.csproj

.PHONY: upgrade
upgrade:
	dotnet outdated --upgrade