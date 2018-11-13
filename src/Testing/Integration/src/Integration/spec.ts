// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

/* import { add } from '../support/add' */

describe('TypeScript', () => {

    it('tests our site', () => {
        cy.visit('http://localhost:5000/account/signin?ReturnUrl=%2F')  // change URL to match your dev URL
        cy.get('#username').type('johndoe')
        cy.get('#password').type('123_Password{enter}')
    })

     // enable once we release updated TypeScript definitions
    it('has Cypress object type definition', () => {
        expect(Cypress.version).to.be.a('string')
    })

})
