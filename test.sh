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

echo "🚀 Creating a solution with the template"
dotnet new modular-monolith-host -n $SOLUTION -o $OUTPUT_FOLDER

dotnet new modular-monolith-module -n $MODULE1 -o $OUTPUT_FOLDER
# dotnet new modular-monolith-module -n $MODULE2 -o $OUTPUT_FOLDER
# dotnet new modular-monolith-module -n $MODULE3 -o $OUTPUT_FOLDER
# dotnet sln ./output/$SOLUTION.sln add ./output/src/$MODULE1/*
# dotnet sln ./output/$SOLUTION.sln add ./output/src/$MODULE2/*
# dotnet sln ./output/$SOLUTION.sln add ./output/src/$MODULE3/*

echo "✅ Solution created"

echo "🚀 Building the solution"
dotnet build $OUTPUT_FOLDER/$SOLUTION.sln
dotnet test $OUTPUT_FOLDER/$SOLUTION.sln
dotnet run --project $OUTPUT_FOLDER/src/Host/Host.csproj