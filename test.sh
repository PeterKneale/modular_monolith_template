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

echo "🚀 Building solution"
dotnet build $OUTPUT_FOLDER/$SOLUTION.sln
dotnet test $OUTPUT_FOLDER/$SOLUTION.sln
dotnet run --project $OUTPUT_FOLDER/src/Host/Host.csproj