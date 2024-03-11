# Temperature Control

This service app measures CPU and GPU temperatures on the device and triggers dangerous temperatures.

## License

This project is licensed under the [Creative Commons Attribution 4.0 International License]

## Features

- Collecting the values of the hardware on the device.
- Preventing heat damage to the device.

## Prerequisites

Before you begin, ensure you have met the following requirements:

- **Windows Operating System:** This project is designed to run on Windows. It may not be compatible with other operating systems.
- **Visual Studio:** You need to have Visual Studio installed on your machine. The project is developed using Visual Studio.
- **Administrator Access:** You may need administrator access to install and configure the Windows service.
- **.NET Framework:** Ensure that you have the required version of the .NET Framework installed. This project may target a specific version of the .NET Framework, so make sure you have it installed on your machine.
- **Service Account:** Decide on the service account under which the Windows service will run. This account should have appropriate permissions for the tasks performed by the service.
- **Dependencies:** If your service relies on external dependencies or libraries, ensure they are available and properly configured.
- **Configuration Settings:** Familiarize yourself with any configuration settings required by the service, such as connection strings, application settings, or any custom configurations.
- **Testing Environment:** Set up a testing environment where you can deploy and test the service before deploying it to production.

## Usage

To use this Windows service project, follow these steps:

1. **Clone the Repository:** First, clone the repository to your local machine using the following command:

    ```bash
    git clone https://github.com/muratbayraktarr/TemperatureControl.git
    ```

2. **Open Project in Visual Studio:** Open the project in Visual Studio by navigating to the project directory and double-clicking the solution file (`TemperatureControl.sln`).

3. **Build the Solution:** Build the solution in Visual Studio by selecting `Build > Build Solution` from the menu. This will compile the project and resolve any dependencies.

4. **Configure Service Settings:** If the Windows service requires any configuration settings, such as connection strings or application settings, make sure to configure them appropriately. You can usually find these settings in the `app.config` or `web.config` file depending on your project type.

5. **Install the Service:** To install the service, you'll need to use the `installutil` tool provided by .NET. Open Command Prompt as Administrator and navigate to the directory where your service executable (`.exe`) is located. Then run the following command:

    ```bash
    InstallUtil.exe -i TemperatureControl.exe
    ```


6. **Start the Service:** After installation, you can start the service either from the Services management console (`services.msc`) or by running the following command in Command Prompt as Administrator:

    ```bash
    net start TemperatureControl
    ```


7. **Verify Service Operation:** Once the service is started, verify that it's running correctly by checking its status in the Services management console or by checking log files or any other means your service uses for reporting its status or operations.

8. **Test Service Functionality:** Test the functionality of your service to ensure it's performing the intended tasks correctly.

9. **Stop and Uninstall the Service:** To stop the service, you can use the Services management console or run the following command:

    ```bash
    net stop TemperatureControl
    ```

    To uninstall the service, run the following command:

    ```bash
    InstallUtil.exe -u TemperatureControl.exe
    ```


10. **Cleanup:** Optionally, clean up any temporary files or configurations created during testing or installation.

**Contact:**

If you have any questions or feedback, feel free to reach out to me at: [muratt.bayraktarrr@gmail.com](mailto:muratt.bayraktarrr@gmail.com).

Contributing

Contributions are welcome! Please fork the repository and create a pull request with your changes.  

