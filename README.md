
# Web Pulse

## Overview

Web Pulse is an asp.net core web application dedicated to monitoring  and managing multiple websites at a time. The intended audience are developers/agencies who manage multiple sites at a time.

Web Pulse 0.1.0 focuses on the intergration of the site and begins the api structure. Web Pulse utilises asp.net core CRUD functionality whilst utilising api calls for pages that require more dynamic functionality.

# Development Process
To keep the codebase safe from unknown changes, Developers must not have access to change the main codebase without review. So the
development is to be done in separate forks and not directly on the main repository. 

## First time development setup
1. [Create a fork](../../fork)
2. Clone the repo you forked and do changes in `main` branch only.
3. Replace database connection string in Web.config file to your own database.
4. Use `Update-Database` to migrate and seed the database. 

## Development Flow
1. Create an [issue](../../issues) in main repo explaining what you are working on to keep track of the bug/feature on Github.
2. While creating an issue, prefix the title with `[BUG] ` or `[FEATURE] ` appropriately and assign yourself to the issue.
3. Write code in the `develop` branch of your cloned repo. For any kind of database changes, use code-first migrations.
4. Test locally
5. Push changes to your forked repo's develop branch. (Respect .gitignore and do not push unnecessary built executables, binaries and Web.config file that replaces the connection string)
6. Create pull request across forked repo and main repo's develop branch.
7. While creating the pull request, mention the id of the bug in the message and explain what you have implemented in the body of pull request message.
8. Once approved, the pull request is merged by the approver.

# Deployment Flow
The main repo must not allow direct code changes. Each change must be reviewed and approved by an approver.

## Approval Process
1. Clone and checkout the developer's `develop` branch on his repo. Test locally and on staging.
2. If it is working fine check for the conflicts in the pull request.
3. If the patch is not working, ask requester to fix the patch and do not accept the pull request.
4. If there are conflicts, tell the requester to fix the conflicts and do not accept the pull request.
5. If everything is fine, merge to `develop` branch.

## Deploying on live
Once the code is working locally and on staging, it should be ready to be released. 

1. [Create a release](../../releases/new). See https://docs.github.com/en/repositories/releasing-projects-on-github/managing-releases-in-a-repository#creating-a-release
2. After creating a release, you can go back to the [release page]((../../releases) and download the zipball or tarball of source code.
3. Build the created release and upload executables to the live.

## Rolling back changes
* To rollback changes on live, you can deploy an older release from the [releases page](../../releases).
* To rollback the changes locally, use `git reset [commit hash here] --hard`
* To rollback the changes on the remote, use `git revert [commit hash here]` and `git push` the new commit.
