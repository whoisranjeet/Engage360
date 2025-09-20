// Load employee details on selection from dropdown
$('#employeeSelect').on('change', function () {
    var selectedEmployeeEmail = $(this).val();
    $('#employeeSearch').val(''); // Reset the search input

    var employeeDetail = employees.find(emp => emp.EmailAddress === selectedEmployeeEmail);

    if (employeeDetail) {
        fillEmployeeForm(employeeDetail);
        $('#initialMessage').hide();
        $('#employeeUpdateForm').show();
    } else {
        resetForm();
    }
});

// Search functionality
function performSearch() {
    var searchTerm = $('#employeeSearch').val().trim().toLowerCase();
    if (searchTerm === '') {
        alert("Please enter a name or email address to search.");
        return;
    }

    $('#employeeSelect').val(''); // Reset the dropdown selection

    var employeeDetail = employees.find(emp =>
        emp.FirstName.toLowerCase().includes(searchTerm) ||
        emp.LastName.toLowerCase().includes(searchTerm) ||
        emp.EmailAddress.toLowerCase().includes(searchTerm)
    );

    if (employeeDetail) {
        fillEmployeeForm(employeeDetail);
        $('#initialMessage').hide();
        $('#employeeUpdateForm').show();
    } else {
        resetForm();
        alert("No results found for the given search term. Please try again.");
    }
}

// Fill the employee form
function fillEmployeeForm(employee) {
    $('#firstName').val(employee.FirstName);
    $('#lastName').val(employee.LastName);
    $('#gender').val(employee.Gender);
    $('#emailAddress').val(employee.EmailAddress);
    $('#emailHidden').val(employee.EmailAddress);
    $('#mobileNumber').val(employee.MobileNumber);
    $('#address').val(employee.Address);
    $('#department').val(employee.Department);
    $('#salary').val(employee.Salary);
}

// Reset form and display initial message
function resetForm() {
    $('#employeeUpdateForm').hide();
    $('#initialMessage').show();
    $('#employeeUpdateForm').find("input[type=text], input[type=email], input[type=number]").val("");
}

$('#searchButton').on('click', function () {
    performSearch();
});

$('#employeeSearch').on('keydown', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault();
        performSearch();
    }
});

$('#employeeSearch').on('input', function () {
    if ($(this).val() === '') {
        resetForm();
        $('#employeeSelect').val('');
    }
});