# Contributing

## TL;DR
One of the easiest ways to contribute is to participate in discussions and discuss issues. You can also contribute by submitting pull requests with code changes. 
Just [follow rules used in Open Source projects on GitHub.com](https://guides.github.com/activities/contributing-to-open-source/).

## How to contribute
Contributions to PineBlog are welcomed in the form of constructive, reproducible bug reports, feature requests that align to the project's goals, 
or better still a PR that's accompanied with passing tests.

If you have general questions or feedback about using PineBlog, PLEASE DON'T CREATE AN ISSUE AND POST TO STACKOVERFLOW INSTEAD.

Please read the [GitHub flow](https://guides.github.com/introduction/flow/) documentation. **TL;DR** there's only one rule: anything in the master branch is always deployable.

### Development
Check if you have the correct version of [.Net Core]((https://dotnet.microsoft.com/download/dotnet-core)) installed, see the `global.json` for the version used.  

Node.js is also required to build and run the project, specifically the `Opw.PineBlog.RazorPages` project. You can install node.js from https://nodejs.org/, and then restart your command prompt or IDE. Make sure you also install the `npm package manager` while installing node.js.

## Bug Reports
If you're reporting a bug, please include a clear description of the issue, the version of PineBlog you're using, and a set of clear repro steps.

Please remember that PineBlog is a free and open-source project provided to the community with zero financial gain to the author(s). 
Any issues deemed to have a negative or arrogant tone will be closed without response.

## Feature Requests
If you're requesting a feature, please include a clear description of the feature.

## Pull Requests
If you've identified a feature/bug fix that aligns to the project goals, or even just an addition to the docs, please submit a Pull Request (PR). 
If applicable, include tests and ensure all tests are passing locally before you commit.

### Squash and Merge
When completing a PR you squash and merge the commits to create a more streamlined Git history in the repository. Work-in-progress commits are helpful when working on a feature branch, but they aren’t necessarily important to retain in the Git history. If you squash these commits into one commit while merging to the default branch, you can retain the original changes with a clear Git history.

The resulting squashed commit message should be a short description of what has been changed and the references to the PR and the resolved issues.
``` txt
Configurable endpoints (#9) #2 #1
```

And the resulting squashed commit message details can some more information.
``` txt
Endpoints are now configurable through `PineBlogOptions.Endpoint`
```

## Creating a new release

1. Update the `CHANGELOG.md`.
3. Create and push a new tag with the version number, e.g. `1.2.3` ([SemVer 2.0.0](https://semver.org/)). **Do not prefix with the version number!**
3. A build will now be triggered for the release.
