# weather_api
REST API that fetches and restructures SMHI wind and temperature data with support for filtering and API key validation.

## Considerations
- Possible requests are for wind and temperature data
- It is possible to filter by station ID and date
- Data from more than 4 months ago is not currently available
- Requesting data without filtering on station or date will give the latest data (last hour), for stations where that data is available
- Requesting data when filtering on just the station, will give all the available data for that station for the last 4 months (as provided by SMHI)
- Requesting data when filtering on a station and a date range will give all the available data for that station and date range (within the last 4 months as provided by SMHI)