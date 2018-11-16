node {
	stage 'Checkout'
		checkout([$class: 'GitSCM', branches: [[name: '**']], doGenerateSubmoduleConfigurations: false, extensions: [], submoduleCfg: [], userRemoteConfigs: [[credentialsId: 'bb8da787-bf1f-49fa-a618-73a15cf10db8', url: 'git@bitbucket.org:Kimi_S/rankedelo.git']]])

	stage 'Build'
		sh 'dotnet restore'
		sh 'dotnet build -c Release'

	stage 'Test'
		sh 'dotnet test --no-build'

}