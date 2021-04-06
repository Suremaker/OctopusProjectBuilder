pipeline {
 agent any
 environment {
  dotnet = 'dotnet.exe'
 }
 stages {
  stage('Checkout') {
   steps {
    git credentialsId: 'userId', url: 'E:/Repositories/OctopusProjectBuilder.git', branch: 'master'
   }
  }
  stage('Restore Packages') {
   steps {
    bat "dotnet restore --configfile NuGet.Config"
   }
  }
  stage('Clean') {
   steps {
    bat 'dotnet clean'
   }
  }
  stage('Build') {
   steps {
    bat 'dotnet build --configuration Release'
   }
  }
  stage('Pack') {
   steps {
    bat 'dotnet pack --no-build --output nupkgs'
   }
  }
 }
}