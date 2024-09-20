# modular_monolith_template

This repo consists of two dotnet templates

- One to create a solution with a host project
  - Web (Setup to load multiple of the below modules)
    - A single layout page
    - Tabler.io css
  - Integration tests

- One to create modules hosted by the above
  - The module consists of
    - Mediatr
    - DDD
    - Dapper
    - Api (Minimal API's)
    - Web (Razor Pages)
    - Unit tests
    - Integration tests

## Installing

### From this repository
```sh
dotnet new install .
```

### From nuget
- TODO

## Quick start

```bash
# Create a solution with a host project
dotnet new modular-monolith-host -n SAAS -o output

# Create modules
dotnet new modular-monolith-module --force -n Products -o output
dotnet new modular-monolith-module --force -n Sales -o output
dotnet new modular-monolith-module --force -n Inventory -o output

# Fix project references
```

## Complete Example
```sh
#!/bin/bash
set -e
echo "🚀 Uninstalling the template"
dotnet new uninstall .
echo "🚀 Installing the template"
dotnet new install .

SOLUTION=SAAS
MODULE1=Tenancy
MODULE2=Products
MODULE3=Search
OUTPUT_FOLDER=output

rm -rf $OUTPUT_FOLDER
mkdir -p $OUTPUT_FOLDER

echo "🚀 Creating solution from template"
dotnet new modular-monolith-host -n $SOLUTION -o $OUTPUT_FOLDER

echo "🚀 Creating modules from template"
dotnet new modular-monolith-module --force -n $MODULE1 -o $OUTPUT_FOLDER
dotnet new modular-monolith-module --force -n $MODULE2 -o $OUTPUT_FOLDER
dotnet new modular-monolith-module --force -n $MODULE3 -o $OUTPUT_FOLDER

dotnet add ./$OUTPUT_FOLDER/src/Host/Host.csproj reference ./$OUTPUT_FOLDER/src/$MODULE1/$MODULE1.Api/
dotnet add ./$OUTPUT_FOLDER/src/Host/Host.csproj reference ./$OUTPUT_FOLDER/src/$MODULE1/$MODULE1.Contracts/
dotnet add ./$OUTPUT_FOLDER/src/Host/Host.csproj reference ./$OUTPUT_FOLDER/src/$MODULE1/$MODULE1.Web/

dotnet add ./$OUTPUT_FOLDER/src/Host/Host.csproj reference ./$OUTPUT_FOLDER/src/$MODULE2/$MODULE2.Api/
dotnet add ./$OUTPUT_FOLDER/src/Host/Host.csproj reference ./$OUTPUT_FOLDER/src/$MODULE2/$MODULE2.Contracts/
dotnet add ./$OUTPUT_FOLDER/src/Host/Host.csproj reference ./$OUTPUT_FOLDER/src/$MODULE2/$MODULE2.Web/

dotnet add ./$OUTPUT_FOLDER/src/Host/Host.csproj reference ./$OUTPUT_FOLDER/src/$MODULE3/$MODULE3.Api/
dotnet add ./$OUTPUT_FOLDER/src/Host/Host.csproj reference ./$OUTPUT_FOLDER/src/$MODULE3/$MODULE3.Contracts/
dotnet add ./$OUTPUT_FOLDER/src/Host/Host.csproj reference ./$OUTPUT_FOLDER/src/$MODULE3/$MODULE3.Web/


echo "Building solution"
dotnet build $OUTPUT_FOLDER/$SOLUTION.sln
dotnet test $OUTPUT_FOLDER/$SOLUTION.sln
dotnet run --project $OUTPUT_FOLDER/src/Host/Host.csproj
```


## References

.NET Templating
- dotnet/templating wiki: https://github.com/dotnet/templating/wiki
- Why setup.sh is necessary https://github.com/dotnet/templating/discussions/7833
