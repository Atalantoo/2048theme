import java.util.ArrayList;
import java.util.List;

import org.springframework.batch.core.Job;
import org.springframework.batch.core.Step;
import org.springframework.batch.core.configuration.annotation.EnableBatchProcessing;
import org.springframework.batch.core.configuration.annotation.JobBuilderFactory;
import org.springframework.batch.core.configuration.annotation.StepBuilderFactory;
import org.springframework.batch.item.ExecutionContext;
import org.springframework.batch.item.ItemReader;
import org.springframework.batch.item.ItemWriter;
import org.springframework.batch.item.file.FlatFileItemReader;
import org.springframework.batch.item.file.FlatFileItemWriter;
import org.springframework.batch.item.file.mapping.BeanWrapperFieldSetMapper;
import org.springframework.batch.item.file.mapping.DefaultLineMapper;
import org.springframework.batch.item.file.transform.BeanWrapperFieldExtractor;
import org.springframework.batch.item.file.transform.DelimitedLineAggregator;
import org.springframework.batch.item.file.transform.DelimitedLineTokenizer;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.autoconfigure.EnableAutoConfiguration;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.io.FileSystemResource;

@Configuration
@EnableBatchProcessing
@EnableAutoConfiguration
public class BatchConfig {

	@Value("${src}")
	String src;
	@Value("${dest}")
	String dest;
	@Value("${id}")
	String id;

	String DELIMITER = ";";
	String[] NAMES = new String[] { "name" };
	String[] NAMES2 = new String[] { "src", "dest" };

	//

	@Autowired
	JobBuilderFactory jobs;

	@Bean
	Job job() {
		return jobs.get("pictures") //
				.start(prepareStep()) //
				.next(pictureStep()) //
				.build();
	}

	@Autowired
	private StepBuilderFactory steps;

	@Bean
	Step prepareStep() {
		return steps.get("prepare").<Prepare, Prepare> chunk(10) //
				.reader(fileReader(src, Prepare.class, NAMES)) //
				.processor(prepareProcessor()) //
				.writer(memoryWriter()).build();
	}

	@Bean
	Step pictureStep() {
		return steps.get("picture").<Picture, Picture> chunk(10) //
				.reader(memoryReader()) //
				.processor(pictureProcessor()) //
				.writer(fileWriter(dest + "\\generator-result.csv", Picture.class, NAMES2)).build();
	}

	//

	List<Picture> inMemoryList = new ArrayList<>();

	public PrepareProcessor prepareProcessor() {
		return new PrepareProcessor( //
				new FileSystemResource(src), //
				new FileSystemResource(dest));
	}

	public PictureProcessor pictureProcessor() {
		return new PictureProcessor();
	}

	//

	private ItemReader<Picture> memoryReader() {
		return new InMemoryReader(inMemoryList);
	}

	private ItemWriter<Prepare> memoryWriter() {
		return new PrepareWriter(inMemoryList);
	}

	private <T> ItemReader<T> fileReader(String src, Class<T> clzz, String[] names) {
		FlatFileItemReader<T> r = new FlatFileItemReader<T>();
		r.setResource(new FileSystemResource(src));
		r.setStrict(false);
		DelimitedLineTokenizer t = new DelimitedLineTokenizer();
		t.setStrict(false);
		t.setDelimiter(DELIMITER);
		t.setNames(names);
		BeanWrapperFieldSetMapper<T> s = new BeanWrapperFieldSetMapper<>();
		s.setTargetType(clzz);
		DefaultLineMapper<T> m = new DefaultLineMapper<T>();
		m.setLineTokenizer(t);
		m.setFieldSetMapper(s);
		r.setLineMapper(m);
		r.open(new ExecutionContext());
		return r;
	}

	private <T> ItemWriter<T> fileWriter(String dest, Class<T> clzz, String[] names) {
		BeanWrapperFieldExtractor<T> f = new BeanWrapperFieldExtractor<>();
		f.setNames(names);
		DelimitedLineAggregator<T> a = new DelimitedLineAggregator<>();
		a.setDelimiter(DELIMITER);
		a.setFieldExtractor(f);
		FlatFileItemWriter<T> w = new FlatFileItemWriter<T>();
		w.setResource(new FileSystemResource(dest));
		w.setShouldDeleteIfEmpty(true);
		w.setLineAggregator(a);
		return w;
	}

}
