# DecorAndHandicraftMerchant
Live link: https://decorandhandicraftmerchant.azurewebsites.net (Website is stopped for now)

Assignment3:
Google login added for users to log in with there Google id.
Assignment 3 focuses on adding cart and payment gateway to the website. There are significant changes to the website view. A profile section is created for users for easy checkout. It is linked to the Name greeting after you log in. The manage identity pages has been removed from there.
All the functionality of this website is now complete. You can add category, subcategory and products and only administrator can create, edit and delete them. Cart, order and order details are created in the process of Shopping. 
All users can see the details of the products and add them to cart, but only registered users can checkout. The cart has continue shopping and checkout buttons.
For checkout, the user needs to complete the profile as well, if the profile is not created, the user will be redirected to create it. If a user tries to recreate an existing profile, he will be directed to the edit page instead of create. The Checkout page shows the details of the profile and a link to edit them. After the payment is made, the cart is cleared and the user is directed to order details. There is also an Order History page for registered users. the administrator can see all the orders but the customer can see only the orders made by them. An anonymous user does not have access to the order history.
I tried going out of the way to integrate profile into my website.

Assignment2:
Register and Login functionality working. Databases set up. Views show only functionality that is Authorized to the user's role.
Extra functionality: 
The CRUD pages for Categories and SubCategories controller are now linked backed to the Categories and SubCategories Indexes through a parameter in RedirectToAction(Still working on it).
When there is no photo added to a category, subcategory or product, a default "Image not available" photo shows up.
The table view for the Index pages have been changed to a grid view.
The details page shows the picture at top and details after that.
The Extra code file just contains extra table views removed from indexes.
Some code is just for testing and demonstration purpose.
Please excuse the Categories and images added.

Assignment1:
The website provides shopping products by categories and then sub categories as of now.

Bonus1: It has been customised by replacing some css rules. 
Bonus2: I have linked the categories to sub-categories using a viewbag and only one controller link is provided because the other controller can be accessed by clicking the category. The sub-categories index will show only the sub-categories in selected category and not all. Similar is the relationship between subcategories and products controller.

DecorAndHandicraftMerchant
This is an online shopping website that aims to sell decorative and handicraft products. The main motto of this site is to use only recycled or waste products for manufacturing the items it sells. The website will have a promising user-friendly interface.

Currently the database for this website has 8 tables:
Categories
Sub-categories
Products
Carts
Orders
Order Details - A kind of join between orders and products
Profiles
Addresses
There are foriegn keys in the database that connect tables with each other mostly with one to many relationships
