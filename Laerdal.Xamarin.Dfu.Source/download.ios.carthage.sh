#!/bin/bash

echo
echo "### DOWNLOAD IOS SOURCE (via Carthage) ###"
echo

mkdir -p iOS_Carthage
echo "github \"NordicSemiconductor/IOS-Pods-DFU-Library\" == 4.9.0" > iOS_Carthage/Cartfile
pushd iOS_Carthage
carthage update --use-xcframeworks --platform iOS
popd

echo
echo "### CREATE FAT LIBRARIES iOSDFULibrary ###"
echo

iphoneos_framework=`find ./iOS_Carthage/Carthage/Build/iOSDFULibrary.xcframework/ -ipath "*arm64_armv7*" -iname "iOSDFULibrary.framework" | head -n 1`
iphonesimulator_framework=`find ./iOS_Carthage/Carthage/Build/iOSDFULibrary.xcframework/ -ipath "*i386_x86_64*" -iname "iOSDFULibrary.framework" | head -n 1`

if [ ! -d "$iphoneos_framework" ]; then
    echo "Failed : $iphoneos_framework does not exist"
    exit 1
fi
if [ ! -d "$iphonesimulator_framework" ]; then
    echo "Failed : $iphonesimulator_framework does not exist"
    exit 1
fi

fat_lib_path="./iOS_Carthage/Carthage/Build/iOSDFULibrary.xcframework/ios-fat"
rm -rf $fat_lib_path
cp -a $(dirname $iphoneos_framework)/. $fat_lib_path

rm -rf $fat_lib_path/iOSDFULibrary.framework/iOSDFULibrary
lipo -create -output $fat_lib_path/iOSDFULibrary.framework/iOSDFULibrary $iphoneos_framework/iOSDFULibrary $iphonesimulator_framework/iOSDFULibrary
lipo -info $fat_lib_path/iOSDFULibrary.framework/iOSDFULibrary

echo
echo "### SHARPIE ###"
echo

sharpie_folder="iOS_Carthage/Sharpie"
sharpie_version=`sharpie -v`
sharpie_output_file=$sharpie_folder/ApiDefinitions.cs

sharpie bind -sdk iphoneos -o $sharpie_folder -f $fat_lib_path/iOSDFULibrary.framework

echo
echo "### CREATE FAT LIBRARIES ZIPFoundation ###"
echo

iphoneos_framework=`find ./iOS_Carthage/Carthage/Build/ZIPFoundation.xcframework/ -ipath "*arm64_armv7*" -iname "ZIPFoundation.framework" | head -n 1`
iphonesimulator_framework=`find ./iOS_Carthage/Carthage/Build/ZIPFoundation.xcframework/ -ipath "*i386_x86_64*" -iname "ZIPFoundation.framework" | head -n 1`

if [ ! -d "$iphoneos_framework" ]; then
    echo "Failed : $iphoneos_framework does not exist"
    exit 1
fi
if [ ! -d "$iphonesimulator_framework" ]; then
    echo "Failed : $iphonesimulator_framework does not exist"
    exit 1
fi

fat_lib_path="./iOS_Carthage/Carthage/Build/ZIPFoundation.xcframework/ios-fat"
rm -rf $fat_lib_path
cp -a $(dirname $iphoneos_framework)/. $fat_lib_path

rm -rf $fat_lib_path/ZIPFoundation.framework/ZIPFoundation
lipo -remove arm64 -output $iphonesimulator_framework/ZIPFoundation $iphonesimulator_framework/ZIPFoundation
lipo -create -output $fat_lib_path/ZIPFoundation.framework/ZIPFoundation $iphoneos_framework/ZIPFoundation $iphonesimulator_framework/ZIPFoundation
lipo -info $fat_lib_path/ZIPFoundation.framework/ZIPFoundation

