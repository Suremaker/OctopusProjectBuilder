pipeline {
 agent any
 environment {
  dotnet = '"C:\\Program Files\\dotnet\\dotnet.exe"'
 }
 stages {
  stage('Checkout') {
   steps {
    git url: 'E:/Repositories/OctopusProjectBuilder.git', branch: 'master'
   }
  }
  stage('Restore Packages') {
   steps {
    bat "$dotnet restore --configfile NuGet.Config"
   }
  }
  stage('Clean') {
   steps {
    bat "$dotnet clean"
   }
  }
  stage('Build') {
   steps {
    bat "$dotnet build --configuration Release"
   }
  }
  stage('Publish') {
   steps {
    bat "$dotnet --configuration Release --runtime win-x64 --output \"E:/OctopusProjectBuilder/$env.BRANCH_NAME\""
   }
  }
 }
}