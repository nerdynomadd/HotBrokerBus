for foldername in ./src/*; do
    echo "Hey ${foldername}";
    /usr/bin/dotnet pack $foldername;
done