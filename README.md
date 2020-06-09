# Payspace
Payspace interview assessment

## Setup
- Clone the repo, load the solution file in `VS Code` or `Visual Studio 2019` (should work with earlier versions too).
- **Right-click the Solution file, select Properties then click Multiple startup projects. Now select Payspace.API and Payspace.UI.**
- When you press F5, it should start both the UI and API projects. If the API runs on a different port, you'll need to update the `appsettings.json` file appropriately pointing it to the correct address.
- The database will be a Sqllite file called `payspace.db`, and it will automatically by seeded by the contents of a series of `.json` files included in the solution.

## Key notes
- This application is written with SOLID, TDD, DDD and Onion Architecture. 
- The reason there are so many projects is that some artefacts would fit into a nuget package. Using this pattern you can increase build speed, and lock down changes to key classes and also marry a project to a version of a package.
- It's a true REST solution in that the UI is only responsible for the presentational layer. All logic, calculations, final security validation and so on are performed by the seperate API project which can has more functionality than the UI itself.
- You can do everything using Postman or such REST client as the UI does and more. To `log in` to the API generate a JWT token and use that token in protected methods. You must generate the sha256 hash of any password as the API only knows your hashed password.
- There are probably some places data could be cached such as the tax table.
- The default page will list valid logins but you don't need to log in to perform tax calculations. 
- Logging in is only needed to demonstrate security and if you want to tag a tax calculation to a logged in user.
- You'll also need to log in to view the tax calculation logs. Only user `lacan@test.com` has this view permission. (A new menu option in the Admin dropdown will appear).


