﻿using System.IO.IsolatedStorage;
using VaultManager;
using VaultManager.Providers;

namespace Ajiva.PasswordManager.Ui.Maui;

internal class MauiStorageProvider : IStorageProvider
{
    private readonly IsolatedStorageFile _storeStorage;

    public MauiStorageProvider()
    {
        _storeStorage = IsolatedStorageFile.GetUserStoreForApplication();
    }

    /// <inheritdoc />
    public byte[] ReadAllBytes(string path)
    {
        try
        {
            var file = _storeStorage.OpenFile(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var buffer = new byte[file.Length];
            file.Read(buffer);
            file.Close();
            return buffer;
        }
        catch (IsolatedStorageException e)
        {
            if (e.InnerException is FileNotFoundException)
                throw new VaultNotFound(path);
            throw;
        }
    }

    /// <inheritdoc />
    public string ReadAllText(string path)
    {
        try
        {
            var file = _storeStorage.OpenFile(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var reader = new StreamReader(file);
            var text = reader.ReadToEnd();
            file.Close();
            return text;
        }
        catch (IsolatedStorageException e)
        {
            if (e.InnerException is FileNotFoundException)
                throw new VaultNotFound(path);
            throw;
        }
    }

    /// <inheritdoc />
    public void CreateDirectory(string path)
    {
        _storeStorage.CreateDirectory(path);
    }

    /// <inheritdoc />
    public void WriteAllBytes(string path, byte[] data)
    {
        var file = _storeStorage.FileExists(path)
            ? _storeStorage.OpenFile(path, FileMode.Truncate)
            : _storeStorage.OpenFile(path, FileMode.CreateNew);

        file.Write(data);
        file.Close();
    }

    /// <inheritdoc />
    public void WriteAllText(string path, string data)
    {
        var file = _storeStorage.FileExists(path)
            ? _storeStorage.OpenFile(path, FileMode.Truncate)
            : _storeStorage.OpenFile(path, FileMode.CreateNew);

        var writer = new StreamWriter(file);
        writer.Write(data);
        writer.Close();
    }

    /// <inheritdoc />
    public string[] ListFiles(string directory)
    {
        return _storeStorage.GetFileNames(directory + "\\*");
    }

    /// <inheritdoc />
    public string[] ListDirectories(string directory)
    {
        return _storeStorage.GetDirectoryNames(directory + "\\*");
    }
}
