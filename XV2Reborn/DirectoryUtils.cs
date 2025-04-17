using System;
using System.IO;
using System.Windows.Forms;

public static class DirectoryUtils
{
    public static void MergeDirectoriesWithConfirmation(string sourceDir, string destDir)
    {
        if (!Directory.Exists(sourceDir))
            throw new DirectoryNotFoundException($"Source directory '{sourceDir}' does not exist.");
        if (!Directory.Exists(destDir))
            throw new DirectoryNotFoundException($"Destination directory '{destDir}' does not exist.");

        try
        {
            MergeDirectoryRecursive(sourceDir, destDir);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred during merge: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static void MergeDirectoryRecursive(string sourceDir, string destDir)
    {
        // Merge files in the current directory
        foreach (var sourceFile in Directory.GetFiles(sourceDir))
        {
            string fileName = Path.GetFileName(sourceFile);
            string destFile = Path.Combine(destDir, fileName);

            try
            {
                if (File.Exists(destFile))
                {
                    var result = MessageBox.Show(
                        $"A file named '{fileName}' already exists in '{destDir}'.\nDo you want to replace it?",
                        "File Conflict",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Warning
                    );

                    if (result == DialogResult.Yes)
                    {
                        File.Copy(sourceFile, destFile, true);
                    }
                    else if (result == DialogResult.No)
                    {
                        continue; // Skip this file
                    }
                    else
                    {
                        return; // Cancel entire operation
                    }
                }
                else
                {
                    File.Copy(sourceFile, destFile);
                }
            }
            catch (IOException ioEx)
            {
                MessageBox.Show($"Failed to copy '{fileName}': {ioEx.Message}", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Recursively handle subdirectories
        foreach (var sourceSubDir in Directory.GetDirectories(sourceDir))
        {
            string subDirName = Path.GetFileName(sourceSubDir);
            string destSubDir = Path.Combine(destDir, subDirName);

            if (!Directory.Exists(destSubDir))
            {
                Directory.CreateDirectory(destSubDir);
            }

            MergeDirectoryRecursive(sourceSubDir, destSubDir);
        }
    }
}