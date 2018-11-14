// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

/* import { add } from '../support/add' */

describe('TypeScript', () => {

    it('Test Blank Sign In', () => {
        cy.log('*** Get url site')
        cy.visit('http://localhost:5000/account/signin?ReturnUrl=%2F')  // change URL to match your dev URL
        cy.log('*** Click signin button')
        cy.get('.btn').click()
        cy.log('*** Get username error class')
        cy.get('#username').should('have.class', 'input-validation-error')
        cy.log('*** Get password error class')
        cy.get('#username').should('have.class', 'input-validation-error')
    })

    it('Test Sign In', () => {
        cy.log('*** Get url site')
        cy.visit('http://localhost:5000/account/signin?ReturnUrl=%2F')  // change URL to match your dev URL
        cy.log('*** Fill username field')
        cy.get('#username').type('johndoe')
        cy.log('*** Fill password field')
        cy.get('#password').type('123_Password{enter}')
    })

    it('Test Sign Out', () => {
        cy.log('*** Click on user badge')
        cy.get('.dropdown.user.user-menu').click()
        cy.log('*** Click on Sign Out button')
        cy.get('.dropdown.user.user-menu > ul:nth-child(2) > li:nth-child(2) > div:nth-child(2) > a')
            .should('have.attr', 'href')
            .and('include', '/Account/SignOut').then(() => {
                cy.get('.dropdown.user.user-menu > ul:nth-child(2) > li:nth-child(2) > div:nth-child(2) > a').click()
            })
    })

     // enable once we release updated TypeScript definitions
    it('has Cypress object type definition', () => {
        expect(Cypress.version).to.be.a('string')
    })

})
