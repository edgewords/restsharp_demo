Feature: Products
	For checking Product Features

Scenario: Simple GET Product Test
	When I GET product record number 1
	Then I get a 200 response
	And The product is an "iPad"

Scenario Outline: Create a new Product
	When I POST a new product with "<product_name>", <product_price>
	Then I get a 201 response back

	Examples:
		| product_name | product_price |
		| pendrive     | 30            |
		| usb cable    | 10            |

Scenario: Update a Product
	Given That I have just created a new product with name "mouse" and price of 5
	When I Update that product with a name of "USB Headset" and a price of 50
	Then I get a 200 response back
	And that Product is now a "USB Headset"

Scenario: Delete a Product
	When I Delete a product
	Then I get a 200 response

Scenario: Negative Test
	When I GET a product that does not exist
	Then I get a 404 response

