# C# Basics Course Project - Console File Manager

The project is built in **C# .Net Core 6.0**.

# Main features:
1. File structure view. 
Command example:
`ls D:\tmp`  - By default first n files/folders will be shown in the list
`ls D:\tmp -p 2`  - Second n files will be shown in the list
n - number of files per list. Can be configurated in appsetting.config file. 
2. Copy file, folder. 
Command example:
`cp D:\tmp\example.txt D:\tmp\folder\`  - Single file will be copied
`cp D:\tmp\folder D:\tmp\folder2\` - Folder with files will be copied
3. Remove file, folder.
Command example:
`rm D:\tmp\example.txt`  - Single file will be removed
`rm D:\tmp\folder` - Folder with files will be removed
4. Show file/folder info.
Command example:
`file D:\tmp\example.txt`  - Info for a single file will be shown
`file D:\tmp\folder` - Info a single folder will be shown
5. The program remembers the last entered command which will be shown and can be executed by the user with the next launch.
