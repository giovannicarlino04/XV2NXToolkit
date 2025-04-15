using System.Collections.Generic;
using System.Text;
using System;
using System.IO;
public class MSG
{
    public int type = 512; // Always UTF-32
    public MSGEntry[] data;
}

public class MSGEntry
{
    public string NameID;
    public int ID;
    public string[] Lines;
}
public static class MSGHandler
{
    public static MSG Read(string filePath)
    {
        MSG msgFile = new MSG();
        byte[] file = File.ReadAllBytes(filePath);

        int type = BitConverter.ToUInt16(file, 4); // Expected: 0x0200 for UTF-32LE
        msgFile.type = type;

        int entryCount = BitConverter.ToInt32(file, 8);
        int mid2Start = BitConverter.ToInt32(file, 16);
        int mid3Start = BitConverter.ToInt32(file, 20);
        int mid4Start = BitConverter.ToInt32(file, 28);

        msgFile.data = new MSGEntry[entryCount];

        // MID1 – NameID Pointers
        for (int i = 0; i < entryCount; i++)
        {
            int pointer = BitConverter.ToInt32(file, 32 + i * 16);
            int charLength = BitConverter.ToInt32(file, 32 + i * 16 + 4);
            int byteSize = BitConverter.ToInt32(file, 32 + i * 16 + 8);

            string text = Encoding.UTF32.GetString(file, pointer, byteSize).TrimEnd('\0');
            msgFile.data[i] = new MSGEntry { NameID = text };
        }

        // MID2 – IDs
        for (int i = 0; i < entryCount; i++)
        {
            msgFile.data[i].ID = BitConverter.ToInt32(file, mid2Start + i * 4);
        }

        // MID3 + MID4 – Lines
        for (int i = 0; i < entryCount; i++)
        {
            int lineCount = BitConverter.ToInt32(file, mid3Start + i * 8);
            int linePointer = BitConverter.ToInt32(file, mid3Start + i * 8 + 4);

            msgFile.data[i].Lines = new string[lineCount];

            for (int j = 0; j < lineCount; j++)
            {
                int offset = linePointer + j * 16;
                int textPointer = BitConverter.ToInt32(file, offset);
                int charLen = BitConverter.ToInt32(file, offset + 4);
                int byteLen = BitConverter.ToInt32(file, offset + 8);

                string line = Encoding.UTF32.GetString(file, textPointer, byteLen).TrimEnd('\0');
                msgFile.data[i].Lines[j] = line;
            }
        }

        return msgFile;
    }

    public static void Write(MSG msg, string filePath)
    {
        foreach (var entry in msg.data)
        {
            for (int i = 0; i < entry.Lines.Length; i++)
            {
                entry.Lines[i] = entry.Lines[i].Replace("'", "&apos;");
            }
        }

        int entryCount = msg.data.Length;
        int lineCount = 0;
        foreach (var entry in msg.data)
            lineCount += entry.Lines.Length;

        int headerSize = 32;
        int mid1Size = entryCount * 16;
        int mid2Size = entryCount * 4;
        int mid3Size = entryCount * 8;
        int mid4Size = lineCount * 16;
        int headerTotalSize = headerSize + mid1Size + mid2Size + mid3Size + mid4Size;

        byte[] header = new byte[headerTotalSize];
        List<byte> textData = new List<byte>();

        int mid1Start = 32;
        int mid2Start = mid1Start + mid1Size;
        int mid3Start = mid2Start + mid2Size;
        int mid4Start = mid3Start + mid3Size;
        int textStart = mid4Start + mid4Size;

        // Top Header
        header[0] = 0x23; header[1] = 0x4D; header[2] = 0x53; header[3] = 0x47; // #MSG
        header[4] = 0x00; header[5] = 0x02; // 0x0200 => UTF-32 LE
        header[6] = 0x02; header[7] = 0x00;

        ApplyBytes(header, BitConverter.GetBytes(entryCount), 8);     // Entry count
        ApplyBytes(header, BitConverter.GetBytes(32), 12);            // Mid1 start
        ApplyBytes(header, BitConverter.GetBytes(mid2Start), 16);
        ApplyBytes(header, BitConverter.GetBytes(mid3Start), 20);
        ApplyBytes(header, BitConverter.GetBytes(entryCount), 24);
        ApplyBytes(header, BitConverter.GetBytes(mid4Start), 28);

        // Mid1 – NameID
        for (int i = 0; i < entryCount; i++)
        {
            byte[] nameInfo = EncodeString(msg.data[i].NameID, ref textData, textStart);
            ApplyBytes(header, nameInfo, mid1Start + i * 16);
        }

        // Mid2 – IDs
        for (int i = 0; i < entryCount; i++)
        {
            ApplyBytes(header, BitConverter.GetBytes(msg.data[i].ID), mid2Start + i * 4);
        }

        // Mid3 + Mid4 – Lines
        int lineCounter = 0;
        for (int i = 0; i < entryCount; i++)
        {
            int lineAddr = mid4Start + (lineCounter * 16);
            foreach (var line in msg.data[i].Lines)
            {
                byte[] lineInfo = EncodeString(line, ref textData, textStart);
                ApplyBytes(header, lineInfo, mid4Start + (lineCounter * 16));
                lineCounter++;
            }

            ApplyBytes(header, BitConverter.GetBytes(msg.data[i].Lines.Length), mid3Start + i * 8);
            ApplyBytes(header, BitConverter.GetBytes(lineAddr), mid3Start + i * 8 + 4);
        }

        // Combine and Write
        List<byte> final = new List<byte>();
        final.AddRange(header);
        final.AddRange(textData);
        File.WriteAllBytes(filePath, final.ToArray());
    }

    private static byte[] EncodeString(string text, ref List<byte> textBuffer, int baseOffset)
    {
        byte[] encoded = Encoding.UTF32.GetBytes(text + "\0");
        int charCount = text.Length;

        byte[] meta = new byte[16];
        ApplyBytes(meta, BitConverter.GetBytes(baseOffset + textBuffer.Count), 0); // Offset
        ApplyBytes(meta, BitConverter.GetBytes(charCount), 4);                     // Char count
        ApplyBytes(meta, BitConverter.GetBytes(encoded.Length), 8);               // Byte size

        textBuffer.AddRange(encoded);
        return meta;
    }

    private static void ApplyBytes(byte[] target, byte[] data, int offset)
    {
        Array.Copy(data, 0, target, offset, data.Length);
    }
}
