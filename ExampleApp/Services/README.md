# Folder Organization
## Grouping
Group folders by feature and not by type. Grouping by type does not scale well as your application grows. 

https://softwareengineering.stackexchange.com/questions/338597/folder-by-type-or-folder-by-feature

## Shared Logic
Try to avoid "shared" folders or groupings. Its ok code in a feature is later re-used by another feature. While not ideal this happens as projects grow. This is better than constantly refactoring an moving files, adjusting namespaces, etc. At the end of the day we are avoiding spagetti code by making sure our interfaces are "right sized" and can always be injected.

## Keep busines logic seperate
Keep you busines logic in seperate projects so that they can easily be deployed as nuget packages. 

Things like c# API controllers are hard to unit test. All business logic should exist oustide of this. Your code should be able to run as an API, in a console app, etc without a refactor of the business logic.



