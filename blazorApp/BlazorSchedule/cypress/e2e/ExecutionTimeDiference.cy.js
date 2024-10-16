let executionTimeTest5;
let executionTimeTest10;
let executionTimeTest15;
let executionTimeTest20;

function addEmployees(num) {
    for (let i = 1; i <= num; i++) {
        cy.get('[data-test-id="add-employee-menu"]').click();
        cy.get('[data-test-id="add-employee-button"]').click();

        // Fill in the form
        cy.get('[data-test-id="employee-name"]').type(`TE ${i}`); // Unique employee name
        cy.get('[data-test-id="type-of-agreement"]').select(0);
        cy.get('[data-test-id="min-hours"]').type('15');
        cy.get('[data-test-id="position"]').select(0);
        cy.get('[data-test-id="add-position"]').click();
        cy.get('[data-test-id="add-employee-final-button"]').click();
    }
}

function generateSchedule() {
    cy.get('[data-test-id="your-schedule"]').click();
    cy.get('[data-test-id="choose-month"]').select('5');
    cy.get('[data-test-id="choose-year"]').select('2024');

    let startTime = Date.now();
    cy.get('[data-test-id="show-schedule-button"]').click();

    return cy.get('table').should('be.visible').then(() => {
        // Calculate the time difference after generating the schedule
        let endTime = Date.now();
        return endTime - startTime; // Return the execution time
    });
}

describe('Execution time difference', () => {
    beforeEach(() => {
        cy.visit('http://localhost:5103');
        cy.get('[data-test-id="options-page"]').click();
        cy.get('[data-test-id="load-data"]').click();
        cy.get('[data-test-id="add-employee-menu"]').click();

        cy.get('[data-test-id="particular-person"]').each(($person, index) => {
            if (index < 3) {
                cy.wrap($person).click();
                cy.get('[data-test-id="delete-person"]').click();
            }
        });

        cy.get('[data-test-id="particular-person"]').should('have.length', 5);
    });

    it('Execution schedule time with 5 participants', () => {
        generateSchedule().then(executionTime => {
            executionTimeTest5 = executionTime;
            cy.log(`Schedule generation time: ${executionTimeTest5} ms`);
        });
    });

    it('Execution schedule time with 10 participants', () => {
        addEmployees(5); // Add 5 more employees
        cy.get('[data-test-id="particular-person"]').should('have.length', 10);
        generateSchedule().then(executionTime => {
            executionTimeTest10 = executionTime;
            cy.log(`Schedule generation time: ${executionTimeTest10} ms`);
        });
    });

    it('Execution schedule time with 15 participants', () => {
        addEmployees(10); // Add 10 more employees
        cy.get('[data-test-id="particular-person"]').should('have.length', 15);
        generateSchedule().then(executionTime => {
            executionTimeTest15 = executionTime;
            cy.log(`Schedule generation time: ${executionTimeTest15} ms`);
        });
    });

    it('Execution schedule time with 20 participants', () => {
        addEmployees(15); // Add 15 more employees
        cy.get('[data-test-id="particular-person"]').should('have.length', 20);
        generateSchedule().then(executionTime => {
            executionTimeTest20 = executionTime;
            cy.log(`Schedule generation time: ${executionTimeTest20} ms`);
        });
    });

    it('Result', () => {
        cy.log(`executionTimeTest5: ${executionTimeTest5} ms, executionTimeTest10: ${executionTimeTest10} ms, 
            executionTimeTest15: ${executionTimeTest15} ms, executionTimeTest20: ${executionTimeTest20} ms`);
    });
});
