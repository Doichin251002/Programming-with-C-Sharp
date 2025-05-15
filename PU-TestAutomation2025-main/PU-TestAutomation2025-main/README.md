# API Automation testing with .NET

## Project's folder structure
* agenda - summary of each day.    
* postman - collection of the requests and environment variables. Update the Bearer token.   
* src   
   * examples - collection of examples of the base tests
   * gorest-project - a Test project for https://gorest.co.in/

## Required software
https://visualstudio.microsoft.com/ for windows users.   
https://www.jetbrains.com/rider/ for mac/linux users.   
https://www.postman.com/   
https://code.visualstudio.com/   
https://git-scm.com/   
http://gitextensions.github.io/    
https://desktop.github.com/    
https://reqnroll.net/ install the plugin for the chosen IDE

## Cheat sheet
dotnet --list-sdks  
dotnet --list-runtimes   

## Git Helper
https://confluence.atlassian.com/bitbucketserver/basic-git-commands-776639767.html

### Basic Git commands
* git fetch origin -p //To fetch the remote repo, aka to check for updates of the branches
* git pull //To take the changes from the remote to your local branch.
* git checkout {branchName} //To take a remote branch locally.
* git add {fileName} //To start git track of the file.
* git add . //To start git track of all files in the local repository.
* git commit -m "commit message" //To commit your changes to your local repository.
* git push -u origin {branchName} //To deploy your changes to the remote repo. -u parameter should be used when the branch in is missing the remote repository.

## Http - properties of the request methods.
![Screenshot 2023-02-26 095907](https://user-images.githubusercontent.com/125467207/221399054-188f23c5-ad96-4fca-9a42-fe73dbe3ce6c.png)

