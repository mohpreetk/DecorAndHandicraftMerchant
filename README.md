# DecorAndHandicraftMerchant
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

The website provides shopping products by categories and then sub categories as of now.

Bonus1: It has been customised by replacing some css rules. 
Bonus2: I have linked the categories to sub-categories using a viewbag and only one controller link is provided because the other controller can be accessed by clicking the category. The sub-categories index will show only the sub-categories in selected category and not all. Similar is the relationship between subcategories and products controller.
