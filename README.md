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
A good solution for it is to provide the customers a convenient way to track their orders via their smartphones, without the neet to stand at the collect zone - and this is what Trackmeal has achieved.

A typical simulation looks as follows:

1. **An employee** responsible for managing restaurant's menu, adds information about orderable products using the pre-created **adminstrator account**.
2. **Customers** can place their order using **regular accounts**, which simulate self-checkout.
3. The system provides customer the **QR code** redirecting to the page containing live-updated order status and summary. This page requires no authentication and is meant to be accessible from customers' smartphones.
4. The order status can be updated using the management module, accesible from **administrator account**.

