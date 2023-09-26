<h1 align="center">   :zap: <img src="https://avatars.githubusercontent.com/u/1119453?s=200&v=4" width="60" height="60" > <a href="https://github.com/samirans89/browserstack-examples-appium-nunit">BrowserStack Examples Appium NUnit</a>  <img src="https://nunit.org/img/nunit.svg" width="60" height="60" >
 :zap:</h1>


Welcome to BrowserStack NUnit Examples, a sample UI testing framework empowered with **[Appium](https://appium.io/)** and **[NUnit](https://nunit.org/)**. Along with the framework the repository also contains a collection of sample test scripts written for **BrowserStack Demo Application**.

This repository includes a number of **[sample configuration files](/browserstack-examples-appium-nunit/Resources/*.yml)** to run these on tests on BrowserStack platforms including **browsers running on a remote selenium grid such as **[BrowserStack AppAutomate](https://www.browserstack.com/app-automate)** 

<h1></h1>

 ## Tests Included in this Repository
 
 Following are the test cases included in this repository:

| Module   | Test Case                          | Description |
  | ---   | ---                                   | --- |
| [Login](/browserstack-examples-appium-nunit/Tests/LoginTest.cs) |   PerformLoginValidCreds  | Test case Perform Login with valid credentials |
| [Login](/browserstack-examples-appium-nunit/Tests/LoginTest.cs) |   PerformLoginNoCreds  | Test case Perform Login with Blank credentials |
| [Browse](/browserstack-examples-appium-nunit/Tests/BrowseTest.cs) |   CheckItemCount  | Test case validates Item counts on initial state of App |
| [Browse](/browserstack-examples-appium-nunit/Tests/BrowseTest.cs) |   AddNewItem  | Test case adds new item validates newly added item |
| [About](/browserstack-examples-appium-nunit/Tests/AboutTest.cs) |   GoToAboutTab  | Test case navigates to About tab |


  
<h1> </h1>

 # :gear:  [Repository Setup](https://github.com/samirans89/browserstack-examples-appium-nunit)
 
 ## Prerequisites
 Ensure you have the following dependencies installed on the machine
 
 1. Dotnet (3 or above)
 2. Allure Command Line Tool 
 3. [BrowserStack AppAutomate Account](https://www.browserstack.com/app-automate). ![BrowserStack](https://img.shields.io/badge/For-BrowserStackAppAutomate-orange)
 4. Upload Your App on BrowserStack <br>Click [here](https://www.browserstack.com/docs/app-automate/api-reference/appium/apps#upload-an-app) for more details on how to upload an app.

 For this example repository, the sample apps are available here:
 1. Xamarin: bin/xamarin/SimpleApp_Android.apk, bin/xamarin/SimpleApp_iOS.ipa

</br>
 
  ## Setup with Nunit 
 :pushpin: Clone this repository 
 <br/>
  <br/> <br/>
 :pushpin: Navigate to the repository directory
  <br/>
 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
 <code>cd browserstack-examples-appium-nunit</code>
 <br/> <br/>
 :pushpin: Restore the required nuget pakages
  <br/>
 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<code>dotnet restore</code>
<br/> <br/>
:pushpin: Build the project
 <br/>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
 <code>dotnet build</code>
 
 # :wrench:  Repository Configuration
 
The repository is designed to execute test on list of Device's hosted on BrowserStack 
<br>
List of devices could be found [here](https://www.browserstack.com/list-of-browsers-and-platforms/app_automate)

On BrowserStack AppAutomate, you can spin multiple Devices instances in parallel to reduce your over all build time. With NUnit you configure the number concurrent test executions to an optimal count by configuring through [browserstack.yml](/browserstack-examples-appium-nunit/Resources/android_browserstack.yml)

 # :rocket:  Test Execution

 ## Test Execution Prerequisites [![BrowserStack](https://img.shields.io/badge/For-BrowserStackAppAutomate-orange)]()
 
 :pushpin: Create a new [BrowserStack account](https://www.browserstack.com/users/sign_up) or use an existing one.
 <br/> <br/>
 :pushpin: Identify your BrowserStack username and access key from the [BrowserStack App-Automate Dashboard](https://www.browserstack.com/app-automate) and export them as environment variables using the below commands.
 <br/>
 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
  - For Unix-like or Mac machines:

  ```sh
  export BROWSERSTACK_USERNAME=<browserstack-username> &&
  export BROWSERSTACK_ACCESS_KEY=<browserstack-access-key>
  
  ```

  - For Windows:

  ```shell
  set BROWSERSTACK_USERNAME=<browserstack-username>
  set BROWSERSTACK_ACCESS_KEY=<browserstack-access-key>
  
  ```

:page_facing_up: Note: We have configured a list of test capabilities in these [configuration files](/browserstack-examples-appium-nunit/Resources/)

- Copy the capabilities to the root of the project:

  - For \*nix based and Mac machines:
  For Android:
  ```sh
  export BROWSERSTACK_CONFIG_FILE="Resources/android_browserstack.yml"
  ```
   For iOS:
  ```sh
  export BROWSERSTACK_CONFIG_FILE="Resources/ios_browserstack.yml"
  ```

  - For Windows:

  For Android:
  ```sh
  set BROWSERSTACK_CONFIG_FILE="Resources/android_browserstack.yml"
  ```
   For iOS:
  ```sh
  set BROWSERSTACK_CONFIG_FILE="Resources/ios_browserstack.yml"
  ```

Feel free to update the configurations based on your device and test requirements. The exact test capability values can be easily identified using the [Browserstack Capability Generator](https://browserstack.com/app-automate/capabilities)
  
 
 
## Test Execution Profiles

Following are the preconfigured test execution profiles.

  
<table>
 <tr>
  <th width='12%'>Profile</th>
  <th width='10%'>Description</th>
  <th width='10%'>Dotnet Command 
  </th>
  <th width='10%'>Example command</th>
 </tr>
 
 <tr>
  <td>Single Test Execution
  <br>
   <a href="#test-execution-prerequisites--2"><img src="https://img.shields.io/badge/Requires-BrowserStackAppAutomate-orange"/></a>
  </td>
  <td>Runs a single test on a two devices Android & iOS parallely on BrowserStack.</td>
  <td><code>dotnet browserstack-sdk --filter "Method Name"</code></td>
  <td><code>dotnet browserstack-sdk --filter "CheckItemCount"</code></td>
 </tr>
 
  <tr>
  <td>Parallel Execution
   <br>
   <a href="#test-execution-prerequisites--2"><img src="https://img.shields.io/badge/Requires-BrowserStackAppAutomate-orange"/></a>
   </td>
  <td>Concurrently runs the entire test suite on a multiple Devices on BrowserStack.</td>
  <td><code>dotnet browserstack-sdk</code></td>
  <td><code>dotnet browserstack-sdk</code></td>
 </tr>

   <tr>
  <td>Parallel Execution with additonal flags
   <br>
   <a href="#test-execution-prerequisites--2"><img src="https://img.shields.io/badge/Requires-BrowserStackAppAutomate-orange"/></a>
   </td>
  <td>Concurrently runs the entire test suite on a multiple Devices on BrowserStack and logger</td>
  <td><code>dotnet browserstack-sdk --logger:"&lt;NUnit_Logger&gt;"</code></td>
  <td><code>dotnet browserstack-sdk --logger:"nunit;LogFilePath=test-results/results.xml"</code></td>
 </tr>


 </table>
 
 
 
 
 # :card_file_box: [Additional Resources](https://github.com/samirans89/browserstack-examples-appium-nunit#additionalresources)

- View your test results on the [BrowserStack AppAutomate dashboard](https://www.browserstack.com/app-automate)
- Documentation for writing [Automate test scripts in C#](https://www.browserstack.com/docs/app-automate/appium/getting-started/c-sharp)
- Customizing your tests capabilities on BrowserStack using our [test capability generator](https://www.browserstack.com/app-automate/capabilities)
- [List of mobile devices](https://www.browserstack.com/list-of-browsers-and-platforms/app_automate) for automation testing on BrowserStack
- [Using AppAutomate REST API](https://www.browserstack.com/docs/app-automate/api-reference/introduction) to access information about your tests via the command-line interface
- Understand how many parallel sessions you need by using our [Parallel Test Calculator](https://www.browserstack.com/automate/parallel-calculator?ref=github)
- For testing public web applications behind IP restriction, [Inbound IP Whitelisting](https://www.browserstack.com/local-testing/inbound-ip-whitelisting) can be enabled with the [BrowserStack Enterprise](https://www.browserstack.com/enterprise) offering

