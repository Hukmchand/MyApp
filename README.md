# MyApp
Sample App to test GitHub
import java.io.*;
import java.util.*;
import org.apache.commons.csv.CSVFormat;
import org.apache.commons.csv.CSVParser;
import org.apache.commons.csv.CSVRecord;

public class PropertyFileUpdater {
    public static void main(String[] args) {
        // Paths to the relevant folders and files
        String csvFilePath = "path_to_csv_file.csv";
        String devTrunkFolder = "path_to_devtrunk_folder";
        String devSharedFolder = "path_to_devshared_folder";
        String devTrunkEast1Folder = "path_to_devtrunkeast1_folder";

        try {
            // Read CSV file with old West region bucket names and new East region names
            Reader reader = new FileReader(csvFilePath);
            CSVParser csvParser = CSVFormat.DEFAULT.parse(reader);

            for (CSVRecord record : csvParser) {
                // Assuming that the first column contains old West region bucket names,
                // and the second column contains new East region bucket names.
                String oldBucketName = record.get(0);
                String newBucketName = record.get(1);

                // Iterate through properties files in devtrunk folder
                for (File file : new File(devTrunkFolder).listFiles()) {
                    if (file.isFile() && file.getName().endsWith(".properties")) {
                        // Read and parse properties file
                        Properties pr
