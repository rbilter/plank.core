.PHONY: all build test clean pack publish

all: clean build test

build:
	dotnet build

test:
	dotnet test ./tests/Plank.Core.Tests/Plank.Core.Tests.csproj

clean:
	dotnet clean
	rm -rf ./src/Plank.Core/bin/ ./src/Plank.Core/obj/ ./tests/Plank.Core.Tests/bin/ ./tests/Plank.Core.Tests/obj/

pack:
	dotnet pack ./src/Plank.Core/Plank.Core.csproj -c Release -o ./artifacts

publish: pack
	dotnet nuget push ./artifacts/Plank.Core.*.nupkg -k $(NUGET_API_KEY) -s https://api.nuget.org/v3/index.json --skip-duplicate
