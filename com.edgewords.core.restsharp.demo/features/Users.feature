Feature: Users
	For checking User Features

Scenario: Check a single User
	When I request user number 1
	Then I get a 200 response code
	And The item is an iPad

Scenario: Negative Authorization Check
	Given that I am not authorized
	When I get all users
	Then I get a 401 response code