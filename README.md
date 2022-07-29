# Four Key Metrics

Summer 2022 project for Møller Digial implementing Google Four Key Metric using Azure and Jira.

## Description

TODO

## Getting Started

### Dependencies

* Node v18.5.0
* .NET 6.0
* Npm 8.12.1

### Installing
Simply clone the project and navigate into the project directory
```
git clone git@ssh.dev.azure.com:v3/mollerdigital/summer22/devops-metrics
cd devops-metrics
```
#### Backend

```
cd src
mkdir .env
```

Fill .env with following values:
```
MONGO_URL=[MONGO_DB_CONNECTION_URL]
MONGO_DB=weu-dev-metric-mongo-db

APPLICATION_LEVEL=API

```

Finally to run the project:
```
dotnet watch run
```

#### Frontend
```
cd metric_vizualiser
mkdir .env
```

Fill .env with following values:
```
AZURE_AD_CLIENT_ID=cbf3b43e-cc27-4ecb-abfd-f261d0c4a4de
AZURE_AD_CLIENT_SECRET=[CLIENT_SECRET]
AZURE_AD_TENANT_ID=a1e38214-9521-4c2a-a29a-4fa0a4457a5c
NEXT_PUBLIC_BACKEND_URL=http://localhost:5118
```

Finally install dependencies and run project
```
npm install
npm run dev
```

### CRON
To fetch data the backend needs to be set to `APPLICATION_LEVEL=CRON`. Running the project will then get all data specified in `src/config.json`.

To run the project in cron state, set these varialbes in `src/.env`
```

...

APPLICATION_LEVEL = CRON
AZURE_TOKEN=[AZURE_PAT]

JIRA_USER=[JIRA_USER_EMAIL]
JIRA_TOKEN=[JIRA_PAT]
```

or pass environment variables into docker.

Now simply run the project and the system will start fetching and formatting data.

P.S. This might take a while, so don't be alarmed if the system is taking a while to finish.

## Help

* If there are errors running the frontend application, make sure the backend is running and the connection is set up correctly.

* Azure AD callback URL is `http://localhost:3000/api/auth/callback/azure-ad`

* Changing platform, organization or jiraboard can be done in `src/config.json`

* If there are problems fetching data in frontend, try removing cookies.
## Authors

[@Julie Naalsund](https://github.com/julienaalsund)

[@Zaim Imran](https://github.com/Zenjjim)

## License

The license for this project is under construction
<!-- This project is licensed under the [NAME HERE] License - see the LICENSE.md file for details -->

## Acknowledgments

* [GoogleCloudPlatform - fourkeys](https://github.com/GoogleCloudPlatform/fourkeys)