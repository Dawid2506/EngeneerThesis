describe('Tests', () => {
    beforeEach(() => {
        cy.visit('/');
        cy.get('[data-test-id="options-page"]').click();
    });

    it('easy get', () => {
        cy.get('[data-test-id="load-data"]').should('exist');
    });

    it('cypress-liblary-testing', () => {
        cy.findAllByText('read').should('exist')
    });
});