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
```


## References

.NET Templating
- dotnet/templating wiki: https://github.com/dotnet/templating/wiki
- Why setup.sh is necessary https://github.com/dotnet/templating/discussions/7833
