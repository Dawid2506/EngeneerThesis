let executionTimeTest4;
let executionTimeTest20;

describe('Execution time difference', () => {
    beforeEach(() => {
        cy.visit('http://localhost:5103')
    })

    it('Execution schedule time with 4 participants', () => {
        cy.get('[data-test-id="options-page"]').click()
        cy.get('[data-test-id="load-data"]').click()
        cy.get('[data-test-id="add-employee-menu"]').click()

        // cy.contains('[data-test-id="particular-person"] p', 'Brygida')
        //     .parent()
        //     .click();

        cy.get('[data-test-id="particular-person"]')
            .each(($person, index) => {
                if (index < 4) {
                    // perform an action, e.g., click on the element to remove it
                    cy.wrap($person).click();
                    cy.get('[data-test-id="delete-person"]').click()
                }
            });

        cy.get('[data-test-id="particular-person"]').should('have.length', 4);

        cy.get('[data-test-id="your-schedule"]').click()
        cy.get('[data-test-id="choose-month"]').select('5');
        cy.get('[data-test-id="choose-year"]').select('2024');

        let startTime = Date.now();
        cy.get('[data-test-id="show-schedule-button"]').click()

        cy.get('table').should('be.visible').then(() => {
            // Calculate the time difference after generating the schedule
            let endTime = Date.now();
            executionTimeTest4 = endTime - startTime;

            cy.log(`Schedule generation time: ${executionTimeTest4} ms`);
        });
    })

    it('Execution schedule time with 20 participants', () => {
        cy.get('[data-test-id="options-page"]').click()
        cy.get('[data-test-id="load-data"]').click()
        cy.get('[data-test-id="add-employee-menu"]').click()

        for (let i = 1; i <= 12; i++) {
            cy.get('[data-test-id="add-employee-menu"]').click()
            cy.get('[data-test-id="add-employee-button"]').click()

            // Fill in the form
            cy.get('[data-test-id="employee-name"]').type(`Test Employee ${i}`) // Unique employee name
            cy.get('[data-test-id="type-of-agreement"]').select(0)
            cy.get('[data-test-id="min-hours"]').type('15')
            cy.get('[data-test-id="position"]').select(0)
            cy.get('[data-test-id="add-position"]').click()
            cy.get('[data-test-id="add-employee-final-button"]').click()
        }

        cy.get('[data-test-id="particular-person"]').should('have.length', 20);

        cy.get('[data-test-id="your-schedule"]').click()
        cy.get('[data-test-id="choose-month"]').select('5');
        cy.get('[data-test-id="choose-year"]').select('2024');

        let startTime = Date.now();
        cy.get('[data-test-id="show-schedule-button"]').click()

        cy.get('table').should('be.visible').then(() => {
            // Calculate the time difference after generating the schedule
            let endTime = Date.now();
            executionTimeTest20 = endTime - startTime;

            cy.log(`Schedule generation time: ${executionTimeTest20} ms`);
        });
    })

    it('Result', () => {
        const difference = executionTimeTest20 - executionTimeTest4;
        cy.log(`executionTimeTest1: ${executionTimeTest4} ms and executionTimeTest2: ${executionTimeTest20} ms`);
        cy.log(`Difference in generation time: ${difference} ms`);
    })
})