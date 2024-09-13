#!/bin/bash
set -e
echo "🚀 Uninstalling the template"
dotnet new uninstall .
echo "🚀 Installing the template"
dotnet new install .

NAME=SAAS
MODULE1=Tenancy
MODULE2=Products
MODULE3=Search
OUTPUT_FOLDER=output

rm -rf $OUTPUT_FOLDER
mkdir -p $OUTPUT_FOLDER

echo "🚀 Creating a solution with the template"
dotnet new modular-monolith-host -n $NAME -o $OUTPUT_FOLDER
dotnet new modular-monolith-module -n $MODULE1 -o $OUTPUT_FOLDER
dotnet new modular-monolith-module -n $MODULE2 -o $OUTPUT_FOLDER
dotnet new modular-monolith-module -n $MODULE3 -o $OUTPUT_FOLDER

echo "✅ Solution created"

echo "🚀 Building the solution"
dotnet build $OUTPUT_FOLDER/$NAME.sln
dotnet test $OUTPUT_FOLDER/$NAME.sln
dotnet run --project $OUTPUT_FOLDER/src/Host/Host.csproj