import java.util.List;

import org.springframework.batch.item.ItemWriter;
import org.springframework.core.io.Resource;

import lombok.AllArgsConstructor;
import lombok.NonNull;
import lombok.RequiredArgsConstructor;
import lombok.extern.log4j.Log4j;

@Log4j
@RequiredArgsConstructor
public class PrepareWriter implements ItemWriter<Prepare> {
	@NonNull
	private List<Picture> dest;

	@Override
	public void write(List<? extends Prepare> items) throws Exception {
		log.info("write");
		for (Prepare i : items) {
			dest.addAll(i.getPictures());
		}
	}

}
