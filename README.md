# Symphony Ltd. Online Portal Project

Welcome to Symphony Ltd.'s Online Portal Project repository! 🚀

## Overview

This project aims to create an online portal for Symphony Ltd., a private institute specializing in IT and Software certification classes. The portal intends to streamline various functionalities, including course details, entrance exams, results publication, FAQs, and contact information.

## Technology Stack

- **Framework:** .NET MVC (Model-View-Controller)
- **Database:** SQL (Structured Query Language)
- **Frontend:** HTML, CSS, JavaScript
- **Backend:** C# programming language

## .Net MVC Nugget Packages
![Screenshot of packages used in this Symphony eProject](https://github.com/mesumbinshaukat/EProject-Management-Portal/blob/main/temp/images/Packages.PNG?raw=true)


## Features

**Course Information:** Display a comprehensive list of courses offered by Symphony Ltd., along with covered topics and respective fees.

**Entrance Exam Details:** Provide information about upcoming entrance exams, registration process, and associated fees.

**Exam Results:** Publish results of entrance exams, segregate students based on their performance, and display course fees according to their allocated class.

**FAQ Section:** Address commonly asked questions regarding course enrollment, exam procedures, and facilities offered.

**Contact Information:** List various branch locations of Symphony Ltd. with their respective addresses and contact numbers.

## Database Model

The project utilizes a SQL database model comprising entities such as Course, Student, Entrance Exam, Exam Result, FAQ, and Contact Information, allowing efficient data storage and management.

## How to Use

**Clone the Repository:**
   ```bash
   git clone https://github.com/mesumbinshaukat/EProject-Management-Portal.git
   ```


## Run Project
> Clone this repository to your desktop.

> Open project  solution file `Symphony-LTD.sln`

> Open Package Manager Console ![Package Manager Console](https://github.com/mesumbinshaukat/EProject-Management-Portal/blob/main/temp/images/Package%20Manager%20Console.png?raw=true)

> **❗Note:** Change the `DefaultConnection` in `appsettings.json` according to your SSMS configuation. 

> Run this command to generate the database in the SSMS: 
``` 
add-migration Init
update-database
```

> Build project with `IIS Express`. 

> **Now try out the project. 💥**

## Navigation
- **Dashboard**: `/admin/login`
- **Landing Page**: `/index`