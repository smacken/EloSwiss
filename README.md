# EloSwiss head-to-head tournament system with player rankings

Tournament Generation of paired matches
In order to create a tournament we need to know the players that will be involved.

tournament.json
```json
{
  "Name":  "Tournament 1", 
  "Players": [
      "A", "B", "C", "D"
  ]
}
```

Once created the tournament setup file, run EloSwissCli with the file as --setup value
The tournament rounds, with paired matches will be generated and output at a location 
specified with --output path.

Matches

Upon completion of head-to-head matches the results can be entered along with the generated
tournament to build standings/rankings for players. This is handy after each round.
There is the option to build matches after each round to take into account results from previous
rounds. The idea is like-ranked players will face each-other in subsequent rounds.

Match results are entered in the following format:

```csv
#A plays #B, #B wins
#C plays #D, #C wins
#C plays #B, #B wins
#A plays #D, #D wins
```

Playoffs

Playoff matching can be run at the end of round-robin play.
Based upon player rankings, matches for playoffs
e.g. 1vs8, 2vs7, 3vs6, 4vs5

## Getting Started

1. Run the app by entering the following command in the command shell:

    EloSwissCli
   ```console
    dotnet run
   ```

### Prerequisites

Install the following:

- [.NET Core](https://dotnet.microsoft.com/download).

### Installing

Copy exe from /dist/EloSwissCli.exe

Run EloSwissCli.exe:
- select the tournament setup with players to run.
- include the folder to output the tournament scheme including Paired matches

## Running the tests

xUnit testing

```bash
dotnet test EloSwiss.Test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## Deployment

```
dotnet publish
```

## Contributing

Please read [CONTRIBUTING.md](https://gist.github.com/PurpleBooth/b24679402957c63ec426) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/your/project/tags). 

## Authors

* **Scott Mackenzie** - *Initial work* - [Smacktech](https://github.com/smacken)

See also the list of [contributors](https://github.com/smacken/templated/contributors) who participated in this project.
