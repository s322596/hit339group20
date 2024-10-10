document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        selectable: true,
        select: function (info) {
            // Open modal to create a new booking
            document.getElementById('bookingId').value = '';
            document.getElementById('title').value = '';
            document.getElementById('description').value = '';
            document.getElementById('startTime').value = info.startStr;
            document.getElementById('endTime').value = info.endStr;
            document.getElementById('bookingModal').style.display = 'block';
        },
        eventClick: function (info) {
            // Open modal to edit existing booking
            var event = info.event;
            document.getElementById('bookingId').value = event.id;
            document.getElementById('title').value = event.title;
            document.getElementById('description').value = event.extendedProps.description;
            document.getElementById('startTime').value = event.start.toISOString().slice(0, 16);
            document.getElementById('endTime').value = event.end ? event.end.toISOString().slice(0, 16) : '';
            document.getElementById('bookingModal').style.display = 'block';
        },
        events: '/api/Schedules/GetSchedules'
    });

    calendar.render();
});
