.PHONY: all build test clean

all: clean build test

build:
	dotnet build

test:
	dotnet test ./tests/Plank.Core.Tests/Plank.Core.Tests.csproj

clean:
	dotnet clean
	rm -rf ./src/Plank.Core/bin/ ./src/Plank.Core/obj/ ./tests/Plank.Core.Tests/bin/ ./tests/Plank.Core.Tests/obj/