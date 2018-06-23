#!/usr/bin/env bash

echo "Build Script started.."

echo $Major
echo $Minor
echo $BUILD_BUILDNUMBER

VERSION = $Major.$Minor.$BUILD_BUILDNUMBER
echo $VERSION

INFO_PLIST_FILE=$BUILD_SOURCESDIRECTORY/Src/MoneyFox.Ios/Info.plist
echo = $INFO_PLIST_FILE

echo "Updating version name to $VERSION_NAME in Info.plist"
plutil -replace CFBundleShortVersionString -string $VERSION $INFO_PLIST_FILE
plutil -replace CFBundleVersion -string $VERSION $INFO_PLIST_FILE