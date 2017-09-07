﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using Ionic.Zip;
using Iteedee.ApkReader;

namespace APKHelper
{
    public class ReadApk
    {


        public static ApkInfo ReadApkFromPath(string path)
        {
            byte[] manifestData = null;
            byte[] resourcesData = null;

            var manifest = "AndroidManifest.xml";
            var resources = "resources.arsc";

            //读取apk,通过解压的方式读取
            using (var zip = ZipFile.Read(path))
            {
                using (Stream zipstream = zip[manifest].OpenReader())
                {
                    //将解压出来的文件保存到一个路径（必须这样）
                    using (var fileStream = File.Create(manifest, (int)zipstream.Length))
                    {
                        manifestData = new byte[zipstream.Length];
                        zipstream.Read(manifestData, 0, manifestData.Length);
                        fileStream.Write(manifestData, 0, manifestData.Length);
                    }
                }
                using (Stream zipstream = zip[resources].OpenReader())
                {
                    //将解压出来的文件保存到一个路径（必须这样）
                    using (var fileStream = File.Create(resources, (int)zipstream.Length))
                    {
                        resourcesData = new byte[zipstream.Length];
                        zipstream.Read(resourcesData, 0, resourcesData.Length);
                        fileStream.Write(resourcesData, 0, resourcesData.Length);
                    }
                }
            }


            ApkReader apkReader = new ApkReader();
            ApkInfo info = apkReader.extractInfo(manifestData, resourcesData);
            Console.WriteLine(string.Format("Package Name: {0}", info.packageName));
            Console.WriteLine(string.Format("Version Name: {0}", info.versionName));
            Console.WriteLine(string.Format("Version Code: {0}", info.versionCode));

            Console.WriteLine(string.Format("App Has Icon: {0}", info.hasIcon));
            if (info.iconFileName.Count > 0)
                Console.WriteLine(string.Format("App Icon: {0}", info.iconFileName[0]));
            Console.WriteLine(string.Format("Min SDK Version: {0}", info.minSdkVersion));
            Console.WriteLine(string.Format("Target SDK Version: {0}", info.targetSdkVersion));

            if (info.Permissions != null && info.Permissions.Count > 0)
            {
                Console.WriteLine("Permissions:");
                info.Permissions.ForEach(f =>
                {
                    Console.WriteLine(string.Format("   {0}", f));
                });
            }
            else
                Console.WriteLine("No Permissions Found");

            Console.WriteLine(string.Format("Supports Any Density: {0}", info.supportAnyDensity));
            Console.WriteLine(string.Format("Supports Large Screens: {0}", info.supportLargeScreens));
            Console.WriteLine(string.Format("Supports Normal Screens: {0}", info.supportNormalScreens));
            Console.WriteLine(string.Format("Supports Small Screens: {0}", info.supportSmallScreens));
            return info;
        }

        /*

                public static ApkInfo ReadApkFromPath(string path)
                {
                    byte[] manifestData = null;
                    byte[] resourcesData = null;
                    using (ICSharpCode.SharpZipLib.Zip.ZipInputStream zip = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(File.OpenRead(path)))
                    {
                        using (var filestream = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            ICSharpCode.SharpZipLib.Zip.ZipFile zipfile = new ICSharpCode.SharpZipLib.Zip.ZipFile(filestream);
                            ICSharpCode.SharpZipLib.Zip.ZipEntry item;
                            while ((item = zip.GetNextEntry()) != null)
                            {
                                if (item.Name.ToLower() == "androidmanifest.xml")
                                {
                                    manifestData = new byte[50 * 1024];
                                    using (Stream strm = zipfile.GetInputStream(item))
                                    {
                                        strm.Read(manifestData, 0, manifestData.Length);
                                    }

                                }
                                if (item.Name.ToLower() == "resources.arsc")
                                {
                                    using (Stream strm = zipfile.GetInputStream(item))
                                    {
                                        using (BinaryReader s = new BinaryReader(strm))
                                        {
                                            resourcesData = s.ReadBytes((int)s.BaseStream.Length);

                                        }
                                    }
                                }
                            }
                        }
                    }

                    ApkReader apkReader = new ApkReader();
                    ApkInfo info = apkReader.extractInfo(manifestData, resourcesData);
                    Console.WriteLine(string.Format("Package Name: {0}", info.packageName));
                    Console.WriteLine(string.Format("Version Name: {0}", info.versionName));
                    Console.WriteLine(string.Format("Version Code: {0}", info.versionCode));

                    Console.WriteLine(string.Format("App Has Icon: {0}", info.hasIcon));
                    if(info.iconFileName.Count > 0)
                        Console.WriteLine(string.Format("App Icon: {0}", info.iconFileName[0]));
                    Console.WriteLine(string.Format("Min SDK Version: {0}", info.minSdkVersion));
                    Console.WriteLine(string.Format("Target SDK Version: {0}", info.targetSdkVersion));

                    if (info.Permissions != null && info.Permissions.Count > 0)
                    {
                        Console.WriteLine("Permissions:");
                        info.Permissions.ForEach(f =>
                        {
                            Console.WriteLine(string.Format("   {0}", f));
                        });
                    }
                    else
                        Console.WriteLine("No Permissions Found");

                    Console.WriteLine(string.Format("Supports Any Density: {0}", info.supportAnyDensity));
                    Console.WriteLine(string.Format("Supports Large Screens: {0}", info.supportLargeScreens));
                    Console.WriteLine(string.Format("Supports Normal Screens: {0}", info.supportNormalScreens));
                    Console.WriteLine(string.Format("Supports Small Screens: {0}", info.supportSmallScreens));
                    return info;
                }
         * */
    }
}
