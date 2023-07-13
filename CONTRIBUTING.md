## Contributing

Thanks for your interest! We appreciate any contribution to the project.
This document tries to help you get started. If you have questions please write me a mail (see profile).

## Feature Requests / Bug reports
Is there a feature you would like to see in a future release? Or have you found a bug? Create a new issue and describe it as well as possible.

## Translation
Is your language missing or not fully translated? [Head over to Crowdin and help us](https://crowdin.com/project/money-fox).

## Code
You want to contribute code? Great! Look for issues with the following labels, depending on what you are looking for:

- first-timer-only: 

To quote [firsttimersonly.com](http://www.firsttimersonly.com/): "I'm willing to hold your hand so you can make your first PR. This issue is rather a bit easier than normal. And anyone who's already contributed to open source isn't allowed to touch this one!"

- up-for-grabs:

This issues should be easy to solve and are great to start if you already have contributed to OSS before and are new here.

- mentor:

This means that I can provide you additional information to help getting started and a basic idea how to solve this.

For any issue regardless of the label I want to encourage you to ask if there are any questions. I'm happy to help.

## Pull Requests
Pull requests are the primary mechanism we use to change something on the Money Fox. There is a [detailed documentation ](https://help.github.com/articles/using-pull-requests/) on using pull requests by GitHub itself.

Please make pull requests against the `master` branch.

Also, please ensure you have unit tests written for your changes where possible and that all existing tests are still running.

## Getting Started
To start developing you have to have Visual Studio 2022 or JetBrains Rider installed with the .net maui workload installed.
In order to build the Windows Project, you'll have to create a new test certificate. The easiest way is via the Package.appxmanifest - Packaging - Choose Certificate - "Create...". The values you can choose freely.

### OneDrive Backup
This is only required if you want to create or restore backups. Please ensure that you register an app for the usage with OneDrive and add the ID in the OneDriveAuthenticator. [Link](http://go.microsoft.com/fwlink/p/?LinkId=193157)

You then have to place your ID in the OneDriveAuthenticator Placeholder.
