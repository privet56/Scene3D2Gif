using Scene3DLib.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace Scene3DLib
{
    public class Zipper
    {
        public const long GB_2_2 = 2362232012;  //=2.2 GB
        public const string SUBDIR2COMPRESS = "SUBDIR2COMPRESS";
        public const string SUBDIR2UNCOMPRESS = "SUBDIR2UNCOMPRESS";

        public void Rezip(string inDir, string tempDir, string outDir)
        {
            UiThreadManager m = new UiThreadManager();
            m.RunAsynchronously(() =>
            {
                this.RezipFromDirs(inDir, tempDir, outDir);
            });
        }
        public void RezipFromDirs(string inDir, string tempDir, string outDir)
        {
            checkParams(inDir, tempDir, outDir);
            clearOutputDirs(tempDir, outDir);
            string[] inFiles = Directory.GetFiles(inDir, "*");//do not look at subdirs
            int ifile = 0;
            foreach(string file in inFiles)
            {
                Unzip(file, tempDir, ifile, inFiles.Length);
                ifile++;
            }
            string[] files = Directory.GetFiles(tempDir);
            Debug.WriteLine("unzipped "+ inFiles.Length+ " ZIP files into '" + tempDir + "' (in sum "+ files.Length + " files found)");

            Random rnd = new Random();
            string[] MyRandomArray = files.OrderBy(x => rnd.Next()).ToArray();
            int createdZIPs = Zips(files, outDir, tempDir);
            emptyDir(tempDir);
            Debug.WriteLine("FINISH ... (CREATED ZIPS:" + createdZIPs + ")");
        }

        private void checkParams(string inDir, string tempDir, string outDir)
        {
            if( string.IsNullOrWhiteSpace(inDir)   ||
                string.IsNullOrWhiteSpace(tempDir) ||
                string.IsNullOrWhiteSpace(outDir))
            {
                throw new InvalidOperationException("missing params in("+inDir+"), tempDir("+tempDir+"), outDir("+outDir+")");
            }
            if( (inDir.Length   < 5) ||
                (tempDir.Length < 5) ||
                (outDir.Length  < 5))
            {
                throw new InvalidOperationException("invalid params in(" + inDir + "), tempDir(" + tempDir + "), outDir(" + outDir + ")");
            }

            if( !Directory.Exists(inDir)    ||
                !Directory.Exists(tempDir) ||
                !Directory.Exists(outDir))
            {
                throw new InvalidOperationException("invalid params (dirnf) in(" + inDir + "), tempDir(" + tempDir + "), outDir(" + outDir + ")");
            }
        }

        private void clearOutputDirs(string tempDir, string outDir)
        {
            emptyDir(tempDir);
            emptyDir(outDir);
        }

        private void emptyDir(string dir)
        {
            string[] files = Directory.GetFiles(dir, "*");//do not look at subdirs
            foreach (string file in files)
            {
                File.Delete(file);
            }
            Debug.WriteLine("dir '" + dir + "' emptied (deleted files: "+files.Length+")");
        }

        private int Zips(string[] files, string outDir, string tempDir)
        {
            List<string> files4AZip = new List<string>();
            long size4AZip = 0;
            int zipCounter = 0;
            foreach (string file in files)
            {
                long filesize = (new FileInfo(file)).Length;
                if ( filesize < 3)
                {
                    Debug.WriteLine("file '" + file + "' is empty? (" + filesize + ")");
                }
                size4AZip += filesize;
                files4AZip.Add(file);
                if(size4AZip >= GB_2_2)
                {
                    Zip(files4AZip, outDir + "/vvv.r."+ zipCounter+".sys_", tempDir);
                    Debug.WriteLine("ZIP file '" + outDir + "/vvv.r." + zipCounter + ".sys_" + "' created (content: "+ files4AZip.Count+" files)");
                    zipCounter++;
                    size4AZip = 0;
                    files4AZip.Clear();
                }
            }
            {   //rest...
                Zip(files4AZip, outDir + "/vvv.r." + zipCounter + ".sys_", tempDir);
                Debug.WriteLine("ZIP file '" + outDir + "/vvv.r." + zipCounter + ".sys_" + "' created (content: " + files4AZip.Count + " files) (last zip)");
                zipCounter++;
            }

            return zipCounter;
        }

        private void Zip(List<string> files4AZip, string zipFN, string tempDir)
        {
            Directory.CreateDirectory(tempDir + "/"+ SUBDIR2COMPRESS);
            this.emptyDir(tempDir + "/" + SUBDIR2COMPRESS);
            foreach (string fn in files4AZip)
            {
                string dir = Path.GetDirectoryName(fn);
                string onlyfn = Path.GetFileName(fn);
                File.Move(fn, dir + "/" + SUBDIR2COMPRESS +"/" + onlyfn);
            }
            ZipFile.CreateFromDirectory(tempDir + "/" + SUBDIR2COMPRESS, zipFN);
            this.emptyDir(tempDir + "/" + SUBDIR2COMPRESS);
            Directory.Delete(tempDir + "/" + SUBDIR2COMPRESS);

            /*
            using (FileStream compressedFileStream = File.Create(zipFN))
            {
                using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                {
                    foreach (string fn in files4AZip)
                    {
                        Debug.WriteLine("inserting " + fn + " into " + zipFN+" ...");
                        using (FileStream originalFileStream = (new FileInfo(fn).OpenRead()))
                        {
                            originalFileStream.CopyTo(compressionStream);
                        }
                    }
                }
            }*/
        }

        private void Unzip(string sAbsFNZip, string tempDir, int ZipIndex, int ZipFilesCount)
        {
            Debug.WriteLine("start unzipping "+(ZipIndex+1)+"/"+ZipFilesCount+" file:'" + sAbsFNZip + "' into '" + tempDir + "'");
            //TODO: System.IO.IOException: "Die Datei "d:\temp\rezipper\temp\????" ist bereits vorhanden."
            //System.IO.Compression.ZipFile.ExtractToDirectory(sAbsFNZip, tempDir);

            Directory.CreateDirectory(tempDir + "/" + SUBDIR2UNCOMPRESS);
            this.emptyDir(tempDir + "/" + SUBDIR2UNCOMPRESS);
            System.IO.Compression.ZipFile.ExtractToDirectory(sAbsFNZip, tempDir + "/" + SUBDIR2UNCOMPRESS);

            string[] files = Directory.GetFiles(tempDir + "/" + SUBDIR2UNCOMPRESS, "*");//do not look at subdirs
            foreach (string file in files)
            {
                string targetfn = tempDir + "/" + Path.GetFileName(file);
                if (!File.Exists(targetfn))
                    File.Move(file, targetfn);
                else
                {
                    long filesize = (new FileInfo(file)).Length;
                    long targetfilesize = (new FileInfo(targetfn)).Length;
                    if(filesize == targetfilesize)
                    {
                        File.Delete(file);
                    }
                    else
                    {
                        int i = 0;
                        do
                        {
                            targetfn = tempDir + "/" + i + Path.GetFileName(file);
                        }
                        while (File.Exists(targetfn));
                    }
                }
            }

            this.emptyDir(tempDir + "/" + SUBDIR2UNCOMPRESS);
            Directory.Delete(tempDir + "/" + SUBDIR2UNCOMPRESS);

            Debug.WriteLine("end   " + (ZipIndex + 1) + "/" + ZipFilesCount + " unzipping file:'" + sAbsFNZip + "' into '" + tempDir + "'");
        }
    }
}
