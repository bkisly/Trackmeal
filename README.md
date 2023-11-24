# Trackmeal

## Overview

Trackmeal is a simulation web application which helps the user track their order placed locally in a sample fast-food restaurant.

Main features:

- live-updated order status tracking
- QR codes generation
- ordering products from editable menu via registerable accounts
- intuitive cart page
- creating, reading, updating and deleting available products via pre-created administrator account
- managing orders and their status via administrator account

## Scenario

One of the problems of many fast-food restaurants is, in my opinion, the number of people gathering together at the checkout while awaiting for their orders. 
The reason for it is that usually there is only one, central screen indicating whether an order is ready to collect or not.
A good solution for it is to provide the customers a convenient way to track their orders via their smartphones, without the need to stand at the collect zone - and this is what Trackmeal has achieved.

A typical simulation looks as follows:

1. **An employee** responsible for managing restaurant's menu, adds information about orderable products using the pre-created **adminstrator account**.
2. **Customers** can place their order using **regular accounts**, which simulate self-checkout.
3. The system provides customer the **QR code** redirecting to the page containing live-updated order status and summary. This page requires no authentication and is meant to be accessible from customers' smartphones.
4. The order status can be updated using the management module, accesible from **administrator account**.

## Installation

1. Ensure you have **[Docker](https://docs.docker.com/get-docker/)** installed and running on your system.
2. Create a file named `docker-compose.yml` and paste the following content into it:

```yaml
version: "3.4"

services:
    trackmeal:
        image: "ghcr.io/bkisly/trackmeal:master"
        ports:
          - "5000:80"
        environment:
          ConnectionStrings:DefaultConnection: "Server=db;Database=TrackmealDb;User=sa;Password=<database password>;"
          InitialAdminPassword: <admin password>
        depends_on:
          - db

    db:
        image: "mcr.microsoft.com/mssql/server:2022-latest"
        ports:
          - "1433:1433"
        environment:
          SA_PASSWORD: <database password>
          ACCEPT_EULA: "Y"
```

Replace `<database password>` entries with a password for the database and `<admin password>` with a password for the administrator account. Make sure to enter passwords that are **min. 8 characters long and containing at least one capital letter, one small letter, one number and one symbol**.

3. Run the command `docker compose up` in the directory where you have created `docker-compose.yml` file.
4. Once all services are configured, open your browser and enter `localhost:5000`.

## Usage tips

- Administrator account is created by default with a user name **admin@trackmeal.com**.
- Regular accounts can be created with the registration form.
- Products can be ordered once logged in.
- Adminstrator has access to ***Manage*** subpage, where it's possible to create new products and manage orders.

**Important:** QR code redirection works when the application is deployed to a hosting service and available publically. When launched locally, it's impossible to access the app from another device.
