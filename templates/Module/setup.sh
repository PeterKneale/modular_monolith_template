#!/bin/bash

# Directory to search is the current directory
SEARCH_DIR="."

# Line number to replace
LINE_NUMBER=16
LINE_NUMBER_2=10

# New content for the line
NEW_CONTENT='<ProjectReference Include="..\..\Common\Common.csproj" />'

# Escape special characters for sed
escape_sed_pattern() {
    echo "$1" | sed 's/[.[\*^$()|+?{}]/\\&/g'
}

# Escape the new content
ESCAPED_NEW_CONTENT=$(escape_sed_pattern "$NEW_CONTENT")

# Find all .csproj files and replace the specific line if it contains "Common"
find "$SEARCH_DIR" -type f -name "*.csproj" | while read -r FILE; do
    echo "Processing: $FILE"

    # Check if the line contains "Common"
    if sed -n "${LINE_NUMBER}p" "$FILE" | grep -q "Common"; then
        echo "Match found in line $LINE_NUMBER of: $FILE"

        # Replace the specific line with new content
        sed -i.bak "${LINE_NUMBER}s|.*|${ESCAPED_NEW_CONTENT}|" "$FILE"
        echo "Replaced line $LINE_NUMBER in: $FILE"
    else
        if sed -n "${LINE_NUMBER_2}p" "$FILE" | grep -q "Common"; then
            echo "Match found in line $LINE_NUMBER_2 of: $FILE"

            # Replace the specific line with new content
            sed -i.bak "${LINE_NUMBER_2}s|.*|${ESCAPED_NEW_CONTENT}|" "$FILE"
            echo "Replaced line $LINE_NUMBER_2 in: $FILE"
        else
            echo "No match in line $LINE_NUMBER_2 of: $FILE"
        fi
    fi

    # Check if the line contains "Common"
   
done

echo "DONE!"
