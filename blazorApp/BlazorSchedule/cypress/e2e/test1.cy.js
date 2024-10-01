describe('template spec', () => {
  beforeEach(() => {
    cy.visit('http://localhost:5103')
  })

  it('Make test schedule', () => {
    cy.get('[data-test-id="options-page"]').click()
    cy.get('[data-test-id="load-data"]').click()
    cy.get('[data-test-id="your-schedule"]').click()
    cy.get('[data-test-id="choose-month"]').select('5');
    cy.get('[data-test-id="choose-year"]').select('2024');
    cy.get('[data-test-id="show-schedule-button"]').click()
  })
})