pipeline {
	agent any
	stages {
		stage('Checkout') {
			steps {
				checkout([$class: 'GitSCM', branches: [[name: '**']], doGenerateSubmoduleConfigurations: false, extensions: [], submoduleCfg: [], userRemoteConfigs: [[credentialsId: 'bb8da787-bf1f-49fa-a618-73a15cf10db8', url: 'git@bitbucket.org:Kimi_S/rankedelo.git']]])
			}
		}

		stage('Build') {
			steps {
				sh 'dotnet build'
				sh 'dotnet restore'
				sh 'dotnet build -c Release'
			}
		}

		stage('Unit Test') {
			steps {
				sh 'dotnet test --no-build'
			}
		}
	}
}