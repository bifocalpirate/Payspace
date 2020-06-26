# Payspace
Payspace interview assessment.

## Interview Tips
- So I got through to the final interview using this design but there were some issues they pointed out during the final interview and I'll try to list these below.
- I could have added more unit tests. Granted I wrote this app in two days I could have brushed up on my tax calculation knowledge a bit more so that the code is  production quality. In the post-assessment interview pay very close attention to their feedback, make sure the feedback they give you is in fact for your assessment. But that said in my code there might be, but I have not checked, an issue with the tax calculations at the edge-cases (when the salary falls at the end of a boundary). It happens to be the case that I do have access to an accountant at least let me know my method of calculating the tax when the amount falls on the start of a boundary is correct so I know it's not that. Also use LinkedIn, find the names of their staff, go through those people's repositories, check the test cases and compare your output with what you come up with yourself. If your output matches what they output, and they tell you there are calculation issues with your code well too bad for you.
- In the final interview after constructively criticising your code they will talk you through a design problem. You are given an API exposed to the web, and there are these payloads that are being POSTed to it. What they want is the payloads to be persisted to some data store. and possibly later on you want to retrieve the result of some calculation triggered by the save of that payload. (Sound familiar?). What you have to remember is you don't ever want, in the same POST operation to save payload to the database. But be sure to ask if the save should be asynchronous or not. You have to store the payload on some intermediate queue, have a process pick the item up from that queue and then save it to the db. Typical questions that come out of this is how is the POSTing process going to know the calculation is completed? Throug an event that gets published? What do you use an identifier for the payload, what indexing strategy are you going to use on the table storing the payload. Is that database going to be an RDMS or a document storage thing etc? Be sure to know what a clustered index is because it relates to how the records will be physically stored on disk (assuming that you pass in an incrementing payload id).
- As with any interview it would be a good idea to find out how diverse a company is. You can do this by asking a random question about age ranges (culture fit) and the interviewer will answer you with names closest to his heart.

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


