PREREQUISITES
This application requires these installed in order to have it compiled and run
+ .NET 6 SDK
+ Jetbrains Rider or VS 2022 installed

BRIEF INTRODUCTION
This application does simple tasks as following:
+ Reject/Accept a Vehicle comes in garage
+ Reject/Accept a Vehicle comes out garage
Validation rule use in application:
+ A vehicle can't come in garage if there is parking records of it active in garage (same for coming out if there is an inactive parking record, it will be rejected)
+ For unknow reason, a car come out garage but the current free parking lot state of the garage is at its maximum capacity, it will be rejected

For further improvement:
+ Allow a vehicle preserve a specific parking lot by its level index + lot index



