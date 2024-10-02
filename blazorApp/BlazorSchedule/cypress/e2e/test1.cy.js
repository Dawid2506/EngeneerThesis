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

  it.only('Add employee and make schedule again', () => {
    cy.viewport(1280, 720)

    cy.window().then((win) => {
      console.log("Szerokość okna: " + win.innerWidth);
      console.log("Wysokość okna: " + win.innerHeight);
    });

    //load data again
    cy.get('[data-test-id="options-page"]').click()
    cy.get('[data-test-id="load-data"]').click()
    cy.get('[data-test-id="your-schedule"]').click()

    // loop to add 10 employees
    for (let i = 1; i <= 2; i++) {
      cy.get('[data-test-id="add-employee-menu"]').click()
      cy.get('[data-test-id="add-employee-button"]').click()

      //fill in the form
      cy.get('[data-test-id="employee-name"]').type(`Test Employee ${i}`) // Unique employee name
      cy.get('[data-test-id="type-of-agreement"]').select(0)
      cy.get('[data-test-id="min-hours"]').type('15')
      cy.get('[data-test-id="position"]').select(0)
      cy.get('[data-test-id="add-position"]').click()
      cy.get('[data-test-id="add-employee-final-button"]').click()
    }

    cy.get('[data-test-id="your-schedule"]').click()
    cy.get('[data-test-id="choose-month"]').select('5');
    cy.get('[data-test-id="choose-year"]').select('2024');
    cy.get('[data-test-id="show-schedule-button"]').click()
    
    //cy.get('table th').should('have.length', 52) // 1 column for day, 10 for employees, 1 for "Missing"
    cy.get('table th').each(($th, index) => {
      if (index > 0 && index < 51) {
        cy.wrap($th).should('be.visible')
      }
    })
    
    // Check visibility of the table header elements
    cy.get('table th').each(($th, index) => {
      if (index > 0 && index < 51) {
        // Check if the header is visible in the viewport
        cy.wrap($th).should('be.visible')
          .then(($th) => {
            const offset = $th[0].getBoundingClientRect();
            // Użyj rozmiarów viewport z obiektu okna
            cy.window().then((win) => {
              expect(offset.top).to.be.gte(0); // Top >= 0
              expect(offset.right).to.be.lte(win.innerWidth); // Right <= szerokość okna
            });
          });
      }
    })
  })
})